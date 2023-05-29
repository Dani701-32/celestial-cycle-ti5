using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    [Header("UI")]
    [SerializeField]
    private GameObject inventoryScreen;

    [SerializeField]
    private GameObject slotsContainer;

    [SerializeField]
    private GameObject slotPrefab;
    public int maxInventoryIndex = 2;

    void Awake()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        inventoryScreen.SetActive(false);
    }

    public bool canAdd(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if (value.stackSize < referenceData.maxStack)
            {
                Debug.Log($"Você n pode carregar mais de {referenceData.maxStack} desse item");
                return true;
            }
        }
        else if (inventory.Count < maxInventoryIndex)
        {
            Debug.Log($"Inventário lotado");
            return true;
        }
        return false;
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

    private void UpdateScreen() {   
        foreach (InventoryItem item in inventory)
        {
            GameObject instance = Instantiate(slotPrefab, slotsContainer.transform);
            instance.GetComponent<Image>().sprite = item.data.icon;
        }
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
        inventoryScreen.SetActive(false);
    }
}
