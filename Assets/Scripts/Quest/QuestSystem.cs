using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class QuestSystem : MonoBehaviour, ISerializationCallbackReceiver
{
    [Header("SaveSystem")]
    public string savePath;
    public QuestDatabase database;
    private List<Quest> activeQuestList;
    public List<QuestNPC> npcs;
    public QuestDatabaseLists globalQuest;
    public int questCount = 0;

    [Header("UI Quest System")]
    [SerializeField]
    private GameObject questScreen;

    [SerializeField]
    private GameObject questMenuList;

    [SerializeField]
    private GameObject questSlot,
        trackConfirmation,
        trakedQuestScreen;

    [SerializeField]
    private List<GameObject> slotsList;

    [SerializeField]
    private GameObject questDescriptor;

    [SerializeField]
    private TextMeshProUGUI questTitle,
        questDescriotion,
        questGoals,
        questRewards,
        questCompleted,
        questTrakerTitle,
        questTrakerGoals;

    [SerializeField]
    private Quest currentQuest,
        tempQuest;
    public Quest trakedQuest { get; private set; }
    private QuestSlot currentSlot;
    private bool isUi = false;

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < globalQuest.questContainer.Count; i++) globalQuest.questContainer[i].item = database.GetItem[globalQuest.questContainer[i].itemID];
    }

    public void OnBeforeSerialize() { }

    public void SaveQuests()
    {
        GetAllQuests();
        string saveData = JsonUtility.ToJson(globalQuest, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadQuests()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            string saveData = (string)bf.Deserialize(file);
            JsonUtility.FromJsonOverwrite(saveData, globalQuest);
            file.Close();

            StartQuest();
        }      
    }

    public void GetAllQuests()
    {
        globalQuest = new QuestDatabaseLists();

        foreach (QuestNPC npc in npcs)
        {
            List<QuestDatabaseSave> npcQuestList = npc.questDatabase.questContainer;

            foreach (QuestDatabaseSave item in npcQuestList)
            {
                globalQuest.questContainer.Add(item);
            }
        }
        foreach (Quest playerQuest in activeQuestList)
        {
            QuestStructure questData = playerQuest.data; 
            globalQuest.questContainer.Add(new QuestDatabaseSave(questData.questData.questID, playerQuest.currentIndex, 0, questData, playerQuest.currentNPC.dataNPC));
        }
    }

    public void StartQuest()
    {
        foreach (QuestNPC npc in npcs)
        {
            List<QuestStructure> npcQuestList = new List<QuestStructure>();
            int curStep = 0;

            foreach (QuestDatabaseSave item in globalQuest.questContainer)
            {
                if (item.npcId == npc.NPC.id)
                {
                    npcQuestList.Add(item.item);
                    curStep = item.currentStep;
                }
            }

            npc.LoadAllQuests(npcQuestList, curStep);
        }
        activeQuestList.Clear();
        foreach (QuestDatabaseSave item in globalQuest.questContainer)
            {
                if (item.npcId == 0)
                {
                    AddQuest(item.item, item.currentStep, item.npcData);
                }
            }

    }
    public void NewGameQuests(){
        foreach (QuestNPC npc in npcs)
        {
            List<QuestStructure> npcQuestList = new List<QuestStructure>();
            int curStep = 0;

            foreach (QuestStructure questStructure in database.Items)
            {
                if (questStructure.questData.npcQuest.id == npc.NPC.id)
                {
                    npcQuestList.Add(questStructure);
                }
            }

            npc.LoadAllQuests(npcQuestList, curStep);
        }
        activeQuestList.Clear();

    }
    // Start is called before the first frame update
    void Start()
    {
        questScreen.SetActive(false);
        questDescriptor.SetActive(false);
        activeQuestList = new List<Quest>();
        slotsList = new List<GameObject>();
        trakedQuestScreen.SetActive(false);
        isUi = false;
    }

    // Update is called once per frame
    void Update()
    {
        questCount = activeQuestList.Count;
    }

    public void AddQuest(QuestStructure quest, int currentStep, NPCData npc)
    {
        Quest _quest = new Quest(quest, currentStep);
        _quest.SetNPC(npc);
        activeQuestList.Add(_quest);
        CheckQuests();
    }

    public void UpdateScreen()
    {
        foreach (Quest item in activeQuestList)
        {
            GameObject instance = Instantiate(questSlot, questMenuList.transform);
            instance.GetComponent<QuestSlot>().UpdateQuest(item);
            instance.GetComponent<QuestSlot>().questDescription = questDescriptor;
            slotsList.Add(instance);
        }
    }

    public void OpenDescription(Quest quest, QuestSlot currentSlot)
    {
        this.currentSlot = currentSlot;
        currentQuest = quest;
        tempQuest = currentQuest;
        questTitle.text = quest.data.questData.title;
        questDescriotion.text = quest.data.questData.description;
        questGoals.text = quest.data.GetQuestGoals();
        questRewards.text = quest.data.GetQuestRewardsDescription();

        if (
            trakedQuest != null
            && trakedQuest.data.questData.questID == quest.data.questData.questID
        )
        {
            trackConfirmation.gameObject.SetActive(true);
            return;
        }
        trackConfirmation.gameObject.SetActive(false);
    }

    public void CloseDescription()
    {
        questDescriptor.SetActive(false);
    }

    private void ClearList()
    {
        foreach (GameObject item in slotsList)
        {
            Destroy(item);
        }
        slotsList.Clear();
    }

    public void CheckQuests()
    {
        if (activeQuestList.Count == 0)
            return;

        foreach (Quest item in activeQuestList)
        {
            item.CompleteQuest(0);
            if (item.data.isQuestCompleted)
            {
                questCompleted.text =
                    $"Missão Completa \nEncontre com {item.currentNPC.dataNPC.Name} para pegar a recompensa";
            }
            else
            {
                questCompleted.text = "";
            }
        }
        if (!isUi)
        {
            TrackQuest();
        }
        isUi = false;
        GetAllQuests();
        
        
    }

    public Quest CheckQuests(QuestStructure quest)
    {
        if (activeQuestList.Count == 0)
            return null;

        foreach (Quest item in activeQuestList)
        {
            item.CompleteQuest(0);
            if (item.data.isQuestCompleted)
            {
                return item;
            }
        }

        return null;
    }

    public void CompleteQuest(Quest quest)
    {
        foreach (Quest item in activeQuestList)
        {
            if (item.data.questData.questID == quest.data.questData.questID)
            {
                activeQuestList.Remove(item);
                tempQuest = null;
                trakedQuest = null;
                trakedQuestScreen.gameObject.SetActive(false);
                break;
            }
        }
        if (quest.data.questData.isTutorial)
        {
            StartCoroutine(ResponseDialog());
        }
    }

    public void OpenScreen()
    {
        isUi = true;
        if (slotsList.Count > 0)
        {
            ClearList();
        }
        CheckQuests();
        UpdateScreen();
        questScreen.SetActive(true);
    }

    public void CloseScreen()
    {
        if (slotsList.Count > 0)
        {
            ClearList();
        }
        questScreen.SetActive(false);
        CloseDescription();
    }

    public void TrackQuest()
    {
        if (tempQuest == null)
            return;
        trakedQuest = tempQuest;
        questTrakerTitle.text = trakedQuest.data.questData.title;
        trackConfirmation.gameObject.SetActive(true);
        trakedQuestScreen.gameObject.SetActive(true);
        if (trakedQuest.questCompleted)
        {
            questTrakerTitle.text = $"{trakedQuest.data.questData.title}- Concluído";
            questTrakerGoals.text = $"Retorne para {trakedQuest.currentNPC.dataNPC.Name}";
        }
        else
        {
            questTrakerGoals.text = trakedQuest.data.GetQuestGoals();
        }
        if (currentSlot != null)
        {
            currentSlot.UpdateQuest(trakedQuest);
        }
    }

    private IEnumerator ResponseDialog()
    {
        Debug.Log("Corrotina start");

        yield return new WaitForSeconds(1f);

        GameController.gameController.tutorialCombat.SetActive(true);
        Time.timeScale = 0f;
        GameController.gameController.StopCamera();
        GameController.gameController.player.playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
