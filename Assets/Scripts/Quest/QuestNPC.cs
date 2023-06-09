using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestNPC : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Player player;
    private InputAction interact;
    private NPCDialogue dialogueSystem;

    [SerializeField]
    private GameObject canvas;

    [Header("NPC data")]
    public NPCData NPC;

    [SerializeField]
    private int currentQuestAvailable;

    [Header("NPCQuests")]
    [SerializeField]
    private List<QuestStructure> quests;
    public int currentStep;
    public Quest activeQuest;
    public bool interected = false;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        activeQuest = null;
        gameController = GameController.gameController;
        dialogueSystem = gameController._NPCDialogue;
        interected = false;
        isOpen = false;
        currentStep = 0;
        foreach (QuestStructure quest in quests)
        {
            quest.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.playerMovement.interactAction.triggered)
        {
            interected = true;
            Interact();
        }
    }

    private void Interact()
    {
        if (interected)
        {
            player.playerMovement.enabled = false;
            player.QuestIsOpen = true;

            if (activeQuest == null)
                UpdadeScreen();
            else
            {
                if (activeQuest.data.isQuestCompleted)
                {
                    activeQuest.CompleteQuest(5);
                    UpdadeScreen();
                }
                else
                {
                    UpdadeScreen();
                }
            }
        }
    }

    public void Rewards()
    {
        if (activeQuest != null && activeQuest.data.isQuestCompleted)
        {
            Debug.Log("Recebey Qyest a Quest");
            activeQuest.data.GetRewards();
            gameController.questSystem.CompleteQuest(activeQuest);
            activeQuest.data.Invoke();
            quests.Remove(activeQuest.data);
            activeQuest = null;
        }
        dialogueSystem.CloseScreen();
    }

    private void UpdadeScreen()
    {
        if (quests.Count > 0)
        {
            Debug.Log("Com quest");
            if (!isOpen)
            {
                dialogueSystem.NPCsprite.sprite = NPC.NPCsprite;
                dialogueSystem.textNPCName.text = NPC.Name;
                if (activeQuest != null)
                {
                    currentStep = activeQuest.currentIndex;
                }
                activeQuest = new Quest(quests[currentQuestAvailable], currentStep);
                activeQuest.SetNPC(NPC);
                dialogueSystem.OpenScreen(this);
            }
            else
            {
                dialogueSystem.ProgressDialog();
            }
        }
        else
        {
            Debug.Log("sem quest");
            dialogueSystem.NPCsprite.sprite = NPC.NPCsprite;
            dialogueSystem.textNPCName.text = NPC.Name;
            dialogueSystem.OpenScreenDefault(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            canvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            canvas.SetActive(false);
        }
    }
}
