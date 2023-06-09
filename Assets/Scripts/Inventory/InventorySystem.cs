using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    [SerializeField]
    private List<GameObject> slots;

    [Header("UI Invetário")]
    [SerializeField]
    private GameObject inventoryScreen;

    [SerializeField]
    private GameObject slotsContainer;

    [SerializeField]
    private GameObject slotPrefab;
    public int maxInventoryIndex = 2;

    [Header("UI Descrição")]
    [SerializeField]
    private GameObject descriptionScreen;

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

    void Awake()
    {
        inventory = new List<InventoryItem>();

        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();

        inventory = new List<InventoryItem>();
        slots = new List<GameObject>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    private void Start()
    {
        inventoryScreen.SetActive(false);
        descriptionScreen.SetActive(false);
    }

    public bool canAdd(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if (value.stackSize >= referenceData.maxStack && referenceData.canStack)
            {
                Debug.Log($"Você n pode carregar mais de {referenceData.maxStack} desse item");
                return false;
            }
        }
        else if (inventory.Count >= maxInventoryIndex)
        {
            Debug.Log($"Inventário lotado");
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
            ClearInventory();
            UpdateScreen();
        }
    }

    public void Add(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem;
            switch (referenceData.type)
            {
                case ItemType.Artifact:
                    newItem = new ArtifactItem(referenceData);
                    break;
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
            inventory.Add(newItem);
            itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove()
    {
        if (itemDictionary.TryGetValue(currentItem.data, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(currentItem.data);
                descriptionScreen.SetActive(false);
            }
            ClearInventory();
            UpdateScreen();
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(referenceData);
                descriptionScreen.SetActive(false);
            }
        }
        ClearInventory();
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        foreach (InventoryItem item in inventory)
        {
            GameObject instance = Instantiate(slotPrefab, slotsContainer.transform);
            instance.GetComponent<ItemSlot>().UpdateItem(item);
            instance.GetComponent<ItemSlot>().itemDescriptor = descriptionScreen;
            slots.Add(instance);
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
        buttonText.text = (currentItem.data.canStack) ? "Consumir" : "Equipar";
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

    private void ClearInventory()
    {
        foreach (GameObject item in slots)
        {
            Destroy(item);
        }
        slots.Clear();
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
        if (slots.Count != 0)
        {
            ClearInventory();
        }
        UpdateScreen();
        inventoryScreen.SetActive(true);
    }

    public void CloseScreen()
    {
        if (slots.Count > 0)
        {
            ClearInventory();
        }
        descriptionScreen.SetActive(false);
        inventoryScreen.SetActive(false);
    }
}
