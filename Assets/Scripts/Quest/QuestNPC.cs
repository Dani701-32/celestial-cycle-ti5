using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using System.Linq;


[System.Serializable]
public class QuestDatabaseSave
{
    public int itemID;
    public int currentStep;
    public int npcId;
    public QuestStructure item;
    public NPCData npcData; 
    public QuestDatabaseSave(int _id, int _step, int _npcId, QuestStructure _item, NPCData _npcData)
    {
        itemID = _id;
        item = _item;
        currentStep = _step;
        npcId = _npcId;
        npcData = _npcData; 
    }
}

[System.Serializable]
public class QuestDatabaseLists
{    
    public List<QuestDatabaseSave> questContainer = new List<QuestDatabaseSave>();
}

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
    [SerializeField]
    public QuestDatabaseLists questDatabase;
    public int currentStep;
    public Quest activeQuest;
    public bool interected = false;
    public bool isOpen;

    public void LoadAllQuests(List<QuestStructure> _quests, int _currentStep)
    {
        quests.Clear();
        quests = _quests;
        currentStep = _currentStep;
        if(quests.Count > 0){
            activeQuest = new Quest(quests[0], currentStep);
        }
        
    }
    void Start()
    {
        UpdateDataQuest();

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

    public void UpdateDataQuest()
    {
        questDatabase.questContainer.Clear();
        if(quests.Count == 0) return;
        for (int i = 0; i < quests.Count; i++)
        {
            questDatabase.questContainer.Add(new QuestDatabaseSave(quests[i].questData.questID, currentStep, NPC.id, quests[i], NPC));
        }
    }

    private void Interact()
    {
        if (interected)
        {
            
            GameController.gameController.questSystem.CheckQuests();
            player.playerMovement.enabled = false;
            player.QuestIsOpen = true;

            
            if (activeQuest == null){
                Debug.Log("quest null");
                UpdadeScreen();

            }else
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
                UpdateDataQuest();
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
