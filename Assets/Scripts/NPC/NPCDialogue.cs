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

    [Header("UI QuestNPC - Geral")]
    public GameObject questScreen;

    [Header("UI QuestNPC - Resultados")]
    public GameObject questDescriptor;
    public GameObject questReward,
        questRewardList;
    public TextMeshProUGUI textQuestTitle,
        textQuestDescription,
        textQuestGoals,
        textQuestRewards;

    private QuestNPC currentNPC;
    public GameObject acceptButton,
        refuseButton;
    public bool questAcept = false,
        answered = false,
        completed = false;

    [Header("UI Reward")]
    public GameObject rewardButton;

    // Start is called before the first frame update
    void Start()
    {
        dialogScreen.SetActive(false);
        questScreen.SetActive(false);
        questDescriptor.SetActive(false);
        questReward.SetActive(false);
        questRewardList.SetActive(false);
        questAcept = false;
        answered = false;
        completed = false;
    }

    public void OpenScreen(QuestNPC currentNPC)
    {
        answered = false;
        this.currentNPC = currentNPC;
        Cursor.lockState = CursorLockMode.None;
        dialogScreen.SetActive(true);
        GameController.gameController.player.playerMovement.enabled = false;
        GameController.gameController.StopCamera();
        completed = (currentNPC.activeQuest.currentIndex == 5);
        if (currentNPC.activeQuest.currentIndex == 1)
        {
            questScreen.SetActive(true);
        }
        buttonContinue.SetActive(currentNPC.activeQuest.currentIndex == 0);
        currentNPC.isOpen = true;
        UpdateDialog();
    }

    public void OpenScreenDefault(QuestNPC currentNPC)
    {
        answered = false;
        completed = false;
        this.currentNPC = currentNPC;
        Cursor.lockState = CursorLockMode.None;
        dialogScreen.SetActive(true);
        GameController.gameController.player.playerMovement.enabled = false;
        GameController.gameController.player.QuestIsOpen = true;
        GameController.gameController.StopCamera();
        buttonContinue.SetActive(false);

        UpdateDialog();
    }

    public void CloseScreen()
    {
        currentNPC.isOpen = false;
        currentNPC.currentStep =
            (currentNPC.activeQuest != null) ? currentNPC.activeQuest.currentIndex : 0;
        Cursor.lockState = CursorLockMode.Locked;
        dialogScreen.SetActive(false);
        questScreen.SetActive(false);
        GameController.gameController.ReleaseCamera();
        GameController.gameController.player.playerMovement.enabled = true;
        GameController.gameController.player.QuestIsOpen = false;
        currentNPC = null;
    }

    private void UpdateDialog()
    {
        if (currentNPC.activeQuest != null)
        {
            textNPCDialogue.text = currentNPC.activeQuest.CurrentDialogue();
        }
        else
        {
            if (!currentNPC.isOpen)
            {
                Debug.Log("dsafkjasd");
                textNPCDialogue.text =
                    "Espero poder contar com você novamente no futuro minha jovem";
                currentNPC.isOpen = true;
            }
            else
            {
                CloseScreen();
            }
        }
    }

    public void ProgressDialog()
    {
        if (completed)
        {
            currentNPC.Rewards();
            return;
        }
        if (currentNPC.activeQuest != null && currentNPC.activeQuest.currentIndex == 0 && !answered)
        {
            Debug.Log("Teste 1");
            currentNPC.activeQuest.ProgressQuest();
            buttonContinue.SetActive(false);
            UpdateDialog();
            OpenQuestDialog();
        }
        else if (currentNPC.activeQuest.currentIndex == 5)
        {
            UpdateDialog();
            Debug.Log("Teste 2");
            OpenQuestCompleteDialog();
        }
        else
        {
            if (answered || currentNPC.activeQuest.currentIndex == 4)
            {
                CloseScreen();
            }
            else
            {
                UpdateDialog();
            }
        }
    }

    private void OpenQuestCompleteDialog()
    {
        // questScreen.SetActive(true);

        textQuestTitle.text = currentNPC.activeQuest.data.questData.title;
        textQuestDescription.text = currentNPC.activeQuest.data.questData.description;
        textQuestGoals.text = $"Objetivos:\n{currentNPC.activeQuest.data.GetQuestGoals()}";
        textQuestRewards.text =
            $"Recompensas:\n{currentNPC.activeQuest.data.GetQuestRewardsDescription()}";

        acceptButton.SetActive(false);
        refuseButton.SetActive(false);
        rewardButton.SetActive(true);

        completed = true;
    }

    public void OpenQuestDialog()
    {
        questScreen.SetActive(true);
        questDescriptor.SetActive(true);
        textQuestTitle.text = currentNPC.activeQuest.data.questData.title;
        textQuestDescription.text = currentNPC.activeQuest.data.questData.description;
        textQuestGoals.text = $"Objetivos:\n{currentNPC.activeQuest.data.GetQuestGoals()}";
        textQuestRewards.text =
            $"Recompensas:\n{currentNPC.activeQuest.data.GetQuestRewardsDescription()}";
        acceptButton.SetActive(true);
        refuseButton.SetActive(true);
        rewardButton.SetActive(false);
    }

    public void EmptyQuest() { }

    public void CloseQuestDialog()
    {
        currentNPC.interected = false;
        questScreen.SetActive(false);
    }

    public void AceptQuest()
    {
        currentNPC.activeQuest.data.Invoke();
        GameController.gameController.questSystem.AddQuest(
            currentNPC.activeQuest.data,
            currentNPC.activeQuest.currentIndex,
            currentNPC.NPC
        );

        questScreen.SetActive(false);
        questAcept = true;
        answered = true;
        textNPCDialogue.text = currentNPC.activeQuest.StatusQuest(questAcept);
    }

    public void DeclineQuest()
    {
        questAcept = false;
        answered = true;
        questScreen.SetActive(false);
        textNPCDialogue.text = currentNPC.activeQuest.StatusQuest(questAcept);
    }

    public void CompleteQuest()
    {
        Debug.Log("Complete dialogo");
    }

    private IEnumerator ResponseDialog()
    {
        Debug.Log("Corrotina start");
        textNPCDialogue.text = currentNPC.activeQuest.StatusQuest(questAcept);
        CloseQuestDialog();

        yield return new WaitForSeconds(2.5f);
        CloseScreen();
        if (currentNPC.activeQuest.data.questData.isTutorial)
        {
            GameController.gameController.tutorialArtefact.SetActive(true);
            Time.timeScale = 0f;
            GameController.gameController.StopCamera();
            GameController.gameController.player.playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
