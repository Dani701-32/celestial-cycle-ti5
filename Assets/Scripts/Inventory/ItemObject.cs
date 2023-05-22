using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    private PlayerMovement playerMovement;

    public void OnHandlePickupItem()
    {
        GameController.gameController.inventorySystem.Add(referenceItem);
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (playerMovement.interactAction.triggered)
        {
            OnHandlePickupItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            this.playerMovement = playerMovement;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.playerMovement = null;
        }
    }
}
