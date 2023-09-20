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
    private Image backgroundImage;
    [SerializeField]
    private InventoryItem currentItem;

    [SerializeField]
    private TextMeshProUGUI stack;
    public GameObject itemDescriptor;

    // Start is called before the first frame update
    public void UpdateItem(InventoryItem referenceItem)
    {
        currentItem = referenceItem;
        icon.sprite = currentItem.data.icon;
        stack.text = (referenceItem.stackSize > 1) ? $"{referenceItem.stackSize}" : "";
        if(currentItem.equiped){
            backgroundImage.color = Color.yellow;
        }else {
            backgroundImage.color = Color.white;
        }
    }

    public void ShowDescriotion()
    {
        if (itemDescriptor != null)
        {
            itemDescriptor.SetActive(true);
            if(currentItem.data.type == ItemType.Artifact){
                ArtifactItem item = new ArtifactItem(currentItem.data); 
                GameController.gameController.inventorySystem.OpenDescriptionArtifact(item);
                return;
            }
            GameController.gameController.inventorySystem.OpenDescription(currentItem);
        }
    }
}
