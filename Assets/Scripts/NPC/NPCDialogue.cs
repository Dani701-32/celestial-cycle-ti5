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
    private bool questAcept = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogScreen.SetActive(false);
        questScreen.SetActive(false);
        questAcept = false;
    }

    // Update is called once per frame
    void Update() { }

    public void OpenScreen(QuestNPC currentNPC)
    {
        this.currentNPC = currentNPC;
        Cursor.lockState = CursorLockMode.None;
        dialogScreen.SetActive(true);
        GameController.gameController.player.playerMovement.enabled = false;
        GameController.gameController.StopCamera();
        Debug.Log(currentNPC.activeQuest.currentIndex);
        buttonContinue.SetActive(currentNPC.activeQuest.currentIndex == 0);
        UpdateDialog();
    }

    public void CloseScreen()
    {
        currentNPC.currentStep = currentNPC.activeQuest.currentIndex;
        Cursor.lockState = CursorLockMode.Locked;
        dialogScreen.SetActive(false);
        GameController.gameController.ReleaseCamera();
        GameController.gameController.player.playerMovement.enabled = true;

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
        else
        {
            UpdateDialog();
        }
    }

    public void OpenQuestDialog()
    {
        questScreen.SetActive(true);
        textQuestTitle.text = currentNPC.activeQuest.data.questData.title;
        textQuestDescription.text = currentNPC.activeQuest.data.questData.description;
        textQuestGoals.text = $"Objetivos:\n{currentNPC.activeQuest.data.GetQuestGoals()}";
        textQuestRewards.text = $"Recompensas:\n{currentNPC.activeQuest.data.GetQuestRewards()}";
    }

    public void CloseQuestDialog()
    {
        questScreen.SetActive(false);
    }

    public void AceptQuest()
    {
        GameController.gameController.questSystem.AddQuest(currentNPC.activeQuest.data, currentNPC.activeQuest.currentIndex);
        questAcept = true;
        currentNPC.activeQuest.data.Invoke();
        StartCoroutine(ResponseDialog());
    }

    public void DeclineQuest()
    {
        questAcept = false;
        StartCoroutine(ResponseDialog());
    }

    private IEnumerator ResponseDialog()
    {
        Debug.Log("Corrotina start");
        textNPCDialogue.text = currentNPC.activeQuest.StatusQuest(questAcept);
        CloseQuestDialog();

        yield return new WaitForSeconds(2.5f);
        CloseScreen();
    }
}
