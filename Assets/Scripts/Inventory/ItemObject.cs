using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    private PlayerMovement playerMovement;
    private QuestSystem questSystem;
    private InventorySystem inventory;
    private bool isDratroy = false; 
    private Transform firstChild;

    [SerializeField]
    private GameObject canvas;

    private void Start()
    {
        inventory = GameController.gameController.inventorySystem;
        questSystem = GameController.gameController.questSystem;
        canvas.SetActive(false);
        firstChild = transform.GetChild(0);
        firstChild.gameObject.SetActive(true);
        isDratroy = false;
    }

    public void OnHandlePickupItem()
    {
        if (!inventory.CanAdd(referenceItem))
            return;

        if (referenceItem.type == ItemType.Artifact)
        {
            inventory.AddArtifact(referenceItem);
        }
        else
        {
            inventory.Add(referenceItem);
        }
        questSystem.CheckQuests();
        NotificationSystem.Instance.CallNotification(referenceItem);
        firstChild.gameObject.SetActive(false);
        canvas.SetActive(false);
        isDratroy = true;
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
        if (other.TryGetComponent(out PlayerMovement playerMovement )&& !isDratroy)
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
