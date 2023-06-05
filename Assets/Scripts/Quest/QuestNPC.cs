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

    [Header("NPC data")]
    public NPCData NPC;

    [SerializeField]
    private int currentQuestAvailable;

    [Header("NPCQuests")]
    [SerializeField]
    private List<QuestStructure> quests;
    public int currentStep;
    public Quest activeQuest;
    private bool interected = false;

    // Start is called before the first frame update
    void Start()
    {
        activeQuest = null;
        gameController = GameController.gameController;
        dialogueSystem = gameController._NPCDialogue;
        interected = false;
        currentStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.playerMovement.interactAction.triggered)
        {
            interected = (interected) ? false : true;
            Interact();
        }
    }

    private void Interact()
    {
        if (interected)
        {
            player.playerMovement.enabled = false;

            if (activeQuest == null) UpdadeScreen();
            else
            {
                if (activeQuest.data.isQuestCompleted)
                {
                    
                    activeQuest.CompleteQuest(5);
                    UpdadeScreen();
                    Debug.Log("Completou a Quest");
                }
                else
                {
                    UpdadeScreen();
                    Debug.Log("Não Completou a Quest");
                }
            }
            
        }
        else
        {
            dialogueSystem.CloseScreen();
        }
    }

    private void UpdadeScreen()
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
