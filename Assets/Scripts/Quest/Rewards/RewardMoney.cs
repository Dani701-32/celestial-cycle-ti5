using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "RewardMoney", menuName = "Quest System/Reward/New Money Reward")]
public class RewardMoney : QuestStructure.QuestReward
{
    [SerializeField]
    private float rewardValue;

    public override string GetDescription()
    {
        return $"{rewardValue} {this.rewardDescription}";
    }
}
