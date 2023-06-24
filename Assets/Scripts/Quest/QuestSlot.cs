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
    public Image traked;

    public void UpdateQuest(Quest quest)
    {
        QuestSystem questSystem = GameController.gameController.questSystem;
        bool questTraked = false;
        if (questSystem.trakedQuest != null)
        {
            questTraked = (
                questSystem.trakedQuest.data.questData.questID == quest.data.questData.questID
            );
        }

        this.currentQuest = quest;

        textQuest.text = quest.data.questData.title;
        spriteCompleted.SetActive(this.currentQuest.data.isQuestCompleted);
        traked.gameObject.SetActive(questTraked);
    }

    public void ShowDescription()
    {
        if (questDescription != null)
        {
            questDescription.SetActive(true);
            GameController.gameController.questSystem.OpenDescription(currentQuest, this);
        }
    }
}
