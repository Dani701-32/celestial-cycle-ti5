using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    private PlayerMovement playerMovement;
    private QuestSystem questSystem;
    private InventorySystem inventory;
    [SerializeField] private GameObject canvas; 

    private void Start()
    {
        inventory = GameController.gameController.inventorySystem;
        questSystem = GameController.gameController.questSystem;
        canvas.SetActive(false);
    }

    public void OnHandlePickupItem()
    {
        if (!inventory.canAdd(referenceItem))
            return;
            
        inventory.Add(referenceItem);
        questSystem.CheckQuests();
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.interactAction.triggered)
        {
            OnHandlePickupItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            this.playerMovement = playerMovement;
            canvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.playerMovement = null;
            canvas.SetActive(false);
        }
    }
}
