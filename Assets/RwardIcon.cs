using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RwardIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI nameText;

    public void UpdateRwardIcon(QuestStructure.QuestReward reward)
    {
        icon.sprite = reward.Sprite;
        nameText.text = reward.RewardName;
    }
}
