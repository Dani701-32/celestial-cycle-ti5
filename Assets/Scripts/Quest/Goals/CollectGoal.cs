using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CollectGoal",
    menuName = "Quest System/Goals/New Collect Goal",
    order = 1
)]
public class CollectGoal : QuestStructure.QuestGoal
{
    [SerializeField]
    protected InventoryItemData referenceItemData;

    public override void isCompleted()
    {
        isGoalCompleted = CheckQuantity();
    }

    private bool CheckQuantity()
    {
        InventoryItem dataItem = GameController.gameController.inventorySystem.GetInventoryItem(referenceItemData);
        if (dataItem != null)
        {
            AddToCurrentQuantity(dataItem.stackSize);
            return (dataItem.stackSize >= minimumAmount);
        }
        return false;
    }

    private void AddToCurrentQuantity(int quantity)
    {
        currentAmount = quantity;
    }
}
