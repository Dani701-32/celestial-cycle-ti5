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
        itemDescription,
        buttonText;

    [SerializeField]
    private Image spriteDescription;
    private InventoryItem currentItem;

    void Awake()
    {
        inventory = new List<InventoryItem>();
        slots = new List<GameObject>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        inventoryScreen.SetActive(false);
    }

    private void Start()
    {
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
            Remove();
        }
        else
        {
            currentItem.equiped = (currentItem.equiped) ? false : true;
            Debug.Log($"ItemEquiado: {currentItem.equiped}");
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
            InventoryItem newItem = new InventoryItem(referenceData);
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
            }
        }
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
        buttonText.text = (currentItem.data.canStack) ? "Consumir" : "Equipar";
    }

    private void ClearInventory()
    {
        foreach (GameObject item in slots)
        {
            Destroy(item);
        }
        slots.Clear();
    }

    public void OpenScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        UpdateScreen();
        inventoryScreen.SetActive(true);
    }

    public void CloseScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (slots.Count > 0)
        {
            ClearInventory();
        }
        descriptionScreen.SetActive(false);
        inventoryScreen.SetActive(false);
    }
}
