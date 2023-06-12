using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "RewardMoney", menuName = "Quest System/Reward/New Money Reward")]
public class RewardMoney : QuestStructure.QuestReward
{
    [SerializeField]
    private float rewardValue;

    [SerializeField]
    private MoonPhases moonType;

    [SerializeField]
    private InventoryItemData itemDataReference;

    public override string GetDescription()
    {
        return $"{rewardValue} {this.rewardDescription}";
    }

    public override void GetReward()
    {
        for (int i = 0; i < rewardValue; i++)
        {
            Debug.Log("Add item");
            GameController.gameController.inventorySystem.Add(itemDataReference);
        }
    }
}
