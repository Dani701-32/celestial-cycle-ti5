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
    private GameController gameController;

    [SerializeField]
    protected InventoryItemData referenceItemData;

    public override void isCompleted()
    {
        isGoalCompleted = CheckQuantity();
    }

    private bool CheckQuantity()
    {
        InventoryItem data = gameController.inventorySystem.GetInventoryItem(referenceItemData);
        if (data != null)
        {
            return (data.stackSize >= minimumAmount);
        }
        return false;
    }
}
