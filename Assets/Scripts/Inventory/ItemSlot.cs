using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private InventoryItem currentItem;

    [SerializeField]
    private TextMeshProUGUI stack;
    public GameObject itemDescriptor;

    // Start is called before the first frame update
    public void UpdateItem(InventoryItem referenceItem)
    {
        this.currentItem = referenceItem;
        icon.sprite = currentItem.data.icon;
        stack.text = (referenceItem.stackSize > 1) ? $"{referenceItem.stackSize}" : "";
    }

    public void ShowDescriotion()
    {
        if (itemDescriptor != null)
        {
            itemDescriptor.SetActive(true);
            GameController.gameController.inventorySystem.OpenDescription(currentItem);
        }
    }
}
