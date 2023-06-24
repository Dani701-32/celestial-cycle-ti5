using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    private Quest currentQuest;

    [SerializeField]
    private TextMeshProUGUI textQuest;
    public GameObject questDescription;
    public GameObject spriteCompleted;
    public void UpdateQuest(Quest quest)
    {
        this.currentQuest = quest;
        textQuest.text = quest.data.questData.title;
        if(this.currentQuest.data.isQuestCompleted){
            spriteCompleted.SetActive(true);
        } else {
            spriteCompleted.SetActive(false);
        }
    }

    public void ShowDescription()
    {
        if (questDescription != null)
        {
            questDescription.SetActive(true);
            GameController.gameController.questSystem.OpenDescription(currentQuest);
        }
    }
}
