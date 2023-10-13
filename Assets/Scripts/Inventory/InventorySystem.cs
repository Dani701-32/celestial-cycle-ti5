using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditorInternal.Profiling.Memory.Experimental;

[System.Serializable]
public class InventoryDatabaseSave
{
    public string itemID;
    public int amount;
    public InventoryItemData item;
    public InventoryDatabaseSave(string _id, InventoryItemData _item, int _amount)
    {
        itemID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
public class InventorySystem : MonoBehaviour, ISerializationCallbackReceiver
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    private Dictionary<InventoryItemData, ArtifactItem> artifactDictionary;
    public List<InventoryItem> InventoryItems { get; private set; }
    public List<ArtifactItem> InventoryArtifact { get; private set; }
    private List<GameObject> SlotsItems, SlotsArtifacts;

    [Header("SaveSystem")]
    public string savePath;
    public InventoryDatabase database;
    public List<InventoryDatabaseSave> artifactContainer = new List<InventoryDatabaseSave>();
    public List<InventoryDatabaseSave> itemContainer = new List<InventoryDatabaseSave>();

    [Header("UI Invetário")]
    [SerializeField]
    private GameObject inventoryScreen;

    [SerializeField]
    private GameObject artifactScreen;

    [SerializeField]
    private GameObject slotsItemsContainer,
        slotsArtifactContainer;

    [SerializeField]
    private GameObject slotPrefab;
    public int maxInventoryIndex = 12;

    [Header("UI Descrição Inventário")]
    [SerializeField]
    private GameObject descriptionScreenItem;

    [SerializeField]
    private TextMeshProUGUI itemName,
        itemAspect,
        itemDescription,
        buttonText;

    [SerializeField]
    GameObject removeButton,
        equipeButton,
        unequipeButton;

    [SerializeField]
    private Image spriteDescription;
    private InventoryItem currentItem;

    [Header("Ui Descrição Artefatos")]
    [SerializeField]
    private GameObject descriptionScreenArtifact;
    [SerializeField]
    private Image[] imgArtifacts = new Image[4];

    [SerializeField]
    private TextMeshProUGUI artifactName,
        artifactAspect,
        artifactDescription;

    [SerializeField]
    private Image spriteArtifactDescription;
    private ArtifactItem currentArtifact;
    private GameController controller;


    public void SaveInventory()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadInventory()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < itemContainer.Count; i++) itemContainer[i].item = database.GetItem[itemContainer[i].item.id];

        for (int i = 0; i < artifactContainer.Count; i++) artifactContainer[i].item = database.GetItem[artifactContainer[i].item.id];
    }

    public void OnBeforeSerialize() { }

    void Awake()
    {
        controller = GameController.gameController;
    }

    private void Start()
    {    
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        artifactDictionary = new Dictionary<InventoryItemData, ArtifactItem>();

        InventoryItems = new List<InventoryItem>();
        InventoryArtifact = new List<ArtifactItem>();

        SlotsItems = new List<GameObject>();
        SlotsArtifacts = new List<GameObject>();

        if (SavingLoading.instance.StatusFile())
        {
            
            for (int i = 0; i < itemContainer.Count; i++)
            {
                InventoryItemData referenceData = itemContainer[i].item;
                InventoryItem newItem;

                switch (referenceData.type)
                {
                    case ItemType.Weapon:
                        newItem = new WeaponItem(referenceData);
                        break;
                    case ItemType.Consumable:
                        newItem = new ConsumableItem(referenceData);
                        break;
                    default:
                    case ItemType.Collectable:
                        newItem = new CollectableItem(referenceData);
                        break;
                }
                InventoryItems.Add(newItem);
                itemDictionary.Add(referenceData, newItem);
                newItem.SetStack(itemContainer[i].amount);
            }
            for (int i = 0; i < artifactContainer.Count; i++)
            {
                InventoryItemData referenceData = artifactContainer[i].item;
                ArtifactItem newArtifact = new ArtifactItem(referenceData);

                InventoryArtifact.Add(newArtifact);
                artifactDictionary.Add(referenceData, newArtifact);       
            }

            UpdateScreenArtifact();
            UpdateScreenInventory();
        }
           
        inventoryScreen.SetActive(false);
        artifactScreen.SetActive(false);
        descriptionScreenItem.SetActive(false);
        descriptionScreenArtifact.SetActive(false);

        foreach (Image imgArtifact in imgArtifacts)
        {
            imgArtifact.enabled = false;
        }
    }

    public bool CanAdd(InventoryItemData referenceData)
    {
        if (referenceData.type == ItemType.Artifact)
        {
            return CanAddArtifact(referenceData);
        }
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if (value.stackSize >= referenceData.maxStack && referenceData.canStack)
            {
                Debug.Log($"Você n pode carregar mais de {referenceData.maxStack} desse item");
                return false;
            }
        }
        else if (InventoryItems.Count >= maxInventoryIndex)
        {
            Debug.Log("Inventário lotado");
            return false;
        }
        return true;
    }

    private bool CanAddArtifact(InventoryItemData referenceData)
    {
        if (artifactDictionary.TryGetValue(referenceData, out ArtifactItem value))
        {
            if (value.stackSize >= referenceData.maxStack && referenceData.canStack)
            {
                Debug.Log($"Você n pode carregar mais de {referenceData.maxStack} desse item");
                return false;
            }
        }
        else if (InventoryArtifact.Count >= maxInventoryIndex)
        {
            Debug.Log("artefatos lotado");
            return false;
        }
        return true;
    }

    public void UseItem()
    {
        if (currentItem.data.canStack)
        {
            currentItem.Use();
        }
        else
        {
            if (currentItem.equiped)
            {
                currentItem.Remove();
            }
            else
            {
                currentItem.Use();
            }
            OpenDescription(currentItem);
            ClearInventoryItens();
            UpdateScreenInventory();
        }
    }

    public void AddArtifact(InventoryItemData referenceData)
    {
        for (int i = 0; i < artifactContainer.Count; i++)
        {
            if (artifactContainer[i].itemID == referenceData.id)
            {
                artifactContainer[i].AddAmount(1);
                return;
            }
        }
        if (artifactDictionary.TryGetValue(referenceData, out ArtifactItem value))
        {
            value.AddToStack();
            return;
        }
        ArtifactItem newArtifact = new ArtifactItem(referenceData);
        InventoryArtifact.Add(newArtifact);
        artifactDictionary.Add(referenceData, newArtifact);
    
        artifactContainer.Add(new InventoryDatabaseSave(referenceData.id, referenceData, 1));
    }

    public void Add(InventoryItemData referenceData)
    {      
        if (referenceData.type == ItemType.Artifact){
            AddArtifact(referenceData);
            return;
        }
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem;
            switch (referenceData.type)
            {
                case ItemType.Weapon:
                    newItem = new WeaponItem(referenceData);
                    break;
                case ItemType.Consumable:
                    newItem = new ConsumableItem(referenceData);
                    break;
                default:
                case ItemType.Collectable:
                    newItem = new CollectableItem(referenceData);
                    break;
            }          
            InventoryItems.Add(newItem);
            itemDictionary.Add(referenceData, newItem);           
        }

        for (int i = 0; i < itemContainer.Count; i++)
        {
            if (itemContainer[i].itemID == referenceData.id)
            {
                itemContainer[i].AddAmount(1);
                return;
            }
        }
        itemContainer.Add(new InventoryDatabaseSave(referenceData.id, referenceData, 1));
    }

    public void Remove()
    {
        if (itemDictionary.TryGetValue(currentItem.data, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                InventoryItems.Remove(value);
                itemDictionary.Remove(currentItem.data);
                descriptionScreenItem.SetActive(false);
            }
            ClearInventoryItens();
            UpdateScreenInventory();
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                InventoryItems.Remove(value);
                itemDictionary.Remove(referenceData);
                descriptionScreenItem.SetActive(false);
            }
        }
        ClearInventoryItens();
        UpdateScreenInventory();
    }

    private void UpdateScreenInventory()
    {
        foreach (InventoryItem item in InventoryItems)
        {
            GameObject instance = Instantiate(slotPrefab, slotsItemsContainer.transform);
            instance.GetComponent<ItemSlot>().UpdateItem(item);
            instance.GetComponent<ItemSlot>().itemDescriptor = descriptionScreenItem;
            SlotsItems.Add(instance);
        }
    }

    private void UpdateScreenArtifact()
    {
        foreach (InventoryItem artifact in InventoryArtifact)
        {
            GameObject instance = Instantiate(slotPrefab, slotsArtifactContainer.transform);
            instance.GetComponent<ItemSlot>().UpdateItem(artifact);
            instance.GetComponent<ItemSlot>().itemDescriptor = descriptionScreenArtifact;
            SlotsArtifacts.Add(instance);
        }
    }

    public void OpenDescription(InventoryItem currentItem)
    {
        this.currentItem = currentItem;
        itemName.text = currentItem.data.displayName;
        itemDescription.text = currentItem.data.description;
        spriteDescription.sprite = currentItem.data.icon;
        itemAspect.text =
            (currentItem.data.type == ItemType.Weapon) ? "" : GetMoonPhase(currentItem.data.aspect);
        if (currentItem.equiped)
        {
            equipeButton.SetActive(false);
            unequipeButton.SetActive(true);
            removeButton.GetComponent<Button>().enabled = false;
            return;
        }
        buttonText.text = currentItem.data.canStack ? "Consumir" : "Equipar";
        if (currentItem.data.type != ItemType.Collectable)
        {
            equipeButton.SetActive(true);
            unequipeButton.SetActive(false);
        }
        else
        {
            equipeButton.SetActive(false);
            unequipeButton.SetActive(false);
        }
        removeButton.GetComponent<Button>().enabled = true;
    }

    public void OpenDescriptionArtifact(ArtifactItem currentItem)
    {
        this.currentArtifact = currentItem;
        artifactName.text = currentItem.data.displayName;
        artifactDescription.text = currentItem.data.description;
        spriteArtifactDescription.sprite = currentItem.data.icon;
        artifactAspect.text = GetMoonPhase(currentItem.data.aspect);
        if (currentItem.equiped)
        {
            equipeButton.SetActive(false);
            unequipeButton.SetActive(true);
            removeButton.GetComponent<Button>().enabled = false;
            return;
        }
        buttonText.text = currentItem.data.canStack ? "Consumir" : "Equipar";
        if (currentItem.data.type != ItemType.Collectable)
        {
            equipeButton.SetActive(true);
            unequipeButton.SetActive(false);
        }
        else
        {
            equipeButton.SetActive(false);
            unequipeButton.SetActive(false);
        }
        removeButton.GetComponent<Button>().enabled = true;
    }

    private string GetMoonPhase(MoonPhases moonPhase)
    {
        switch (moonPhase)
        {
            case MoonPhases.NewMoon:
                return "Aspecto da Lua Nova";
            case MoonPhases.FirstQuarter:
                return "Aspecto da Lua Crescente";
            case MoonPhases.FullMoon:
                return "Aspecto da Lua Cheia";
            case MoonPhases.ThirdQuarter:
            default:
                return "Aspecto da Lua Minguante";
        }
    }

    private void ClearInventoryItens()
    {
        foreach (GameObject item in SlotsItems)
        {
            Destroy(item);
        }
        SlotsItems.Clear();
    }

    private void ClearInventoryArtifacts()
    {
        foreach (GameObject item in SlotsArtifacts)
        {
            Destroy(item);
        }
        SlotsArtifacts.Clear();
    }

    public InventoryItem GetInventoryItem(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void OpenScreen()
    {
        if (SlotsItems.Count != 0)
        {
            ClearInventoryItens();
        }
        UpdateScreenInventory();
        inventoryScreen.SetActive(true);
    }

    public void OpenArtifactScreen()
    {
        if (SlotsArtifacts.Count != 0)
        {
            ClearInventoryArtifacts();
        }
        UpdateScreenArtifact();
        artifactScreen.SetActive(true);
    }

    public void CloseScreen()
    {
        if (SlotsItems.Count > 0)
        {
            ClearInventoryItens();
        }
        descriptionScreenItem.SetActive(false);
        inventoryScreen.SetActive(false);
    }

    public void CloseArtifactScreen()
    {
        if (SlotsArtifacts.Count > 0)
        {
            ClearInventoryArtifacts();
        }
        descriptionScreenArtifact.SetActive(false);
        artifactScreen.SetActive(false);
    }

    public void EquipeArtifact(int index)
    {
        if (currentArtifact == null) return;
        if (controller.player.HasArtifactRoster(currentArtifact.data.prefab))
        {
            return;
        }


        if (currentArtifact.Use(index))
        {

            imgArtifacts[index].enabled = true;
            imgArtifacts[index].sprite = currentArtifact.data.icon;
        }

    }

    public void RemoveArtfact(int index)
    {
        Debug.Log("Remove " + index);
        controller.player.RemoveArtifact(index);
        imgArtifacts[index].enabled = false;
        // imgArtifacts[index].sprite = currentArtifact.data.icon;
    }

    public void RemoveCurrentArtifact()
    {
        currentArtifact = null;
    }
}
