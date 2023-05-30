using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI Dialog")]
    [SerializeField]
    private GameObject dialogScreen;
    public Image NPCsprite;
    public TextMeshProUGUI textNPCName,
        textNPCDialogue;
    public GameObject buttonContinue;

    [Header("UI QuestNPC")]
    public GameObject questScreen;
    public TextMeshProUGUI textQuestTitle,
        textQuestDescription,
        textQuestGoals,
        textQuestRewards;

    private QuestNPC currentNPC;

    // Start is called before the first frame update
    void Start()
    {
        dialogScreen.SetActive(false);
        questScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() { }

    public void OpenScreen(QuestNPC currentNPC)
    {
        this.currentNPC = currentNPC;
        Cursor.lockState = CursorLockMode.None;
        dialogScreen.SetActive(true);
    }

    public void CloseScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        dialogScreen.SetActive(false);
    }

    private void UpdateDialog()
    {
        if (currentNPC.activeQuest != null)
        {
            textNPCDialogue.text = currentNPC.activeQuest.CurrentDialogue();
        }
    }

    public void ProgressDialog()
    {
        if (currentNPC.activeQuest != null && currentNPC.activeQuest.currentIndex == 0)
        {
            currentNPC.activeQuest.ProgressQuest();
            buttonContinue.SetActive(false);
            UpdateDialog();
            OpenQuestDialog();
        }
    }

    public void OpenQuestDialog()
    {
        questScreen.SetActive(true);
        textQuestTitle.text = currentNPC.activeQuest.data.questData.title;
        textQuestDescription.text = currentNPC.activeQuest.data.questData.description;
        textQuestGoals.text = currentNPC.activeQuest.data.GetQuestGoals();
        textQuestRewards.text = currentNPC.activeQuest.data.GetQuestRewards();
    }
}
