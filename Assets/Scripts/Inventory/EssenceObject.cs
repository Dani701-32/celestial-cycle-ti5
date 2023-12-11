using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EssenceObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    private PlayerMovement playerMovement;
    private DayNightCycle timeController;
    private QuestSystem questSystem;
    private InventorySystem inventory;
    public MoonPhases moonPhase;
    private bool canPick = false;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject childObject;
    private void Start()
    {
        inventory = GameController.gameController.inventorySystem;
        questSystem = GameController.gameController.questSystem;
        timeController = DayNightCycle.InstanceTime;
        canvas.SetActive(false);
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
        gameObject.SetActive(false);
    }

    void Update()
    {
        CanPick();
        if (playerMovement != null && playerMovement.interactAction.triggered)
        {
            OnHandlePickupItem();
        }
    }
    private void CanPick()
    {
        canPick = moonPhase == timeController.GetCurrentPhase() && timeController.isNight;
        if (childObject != null)
        {
            childObject.SetActive(canPick);
            canvas.SetActive(canPick);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (canPick && other.TryGetComponent(out PlayerMovement playerMovement))
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
