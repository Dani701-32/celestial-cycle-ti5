using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    private List<Quest> activeQuestList;
    public int questCount = 0;

    [Header("UI Quest System")]
    [SerializeField]
    private GameObject questScreen;

    [SerializeField]
    private GameObject questMenuList;

    [SerializeField]
    private GameObject questSlot;

    [SerializeField]
    private List<GameObject> slotsList;

    [SerializeField]
    private GameObject questDescriptor;
    [SerializeField]
    private TextMeshProUGUI questTitle,
        questDescriotion,
        questGoals,
        questRewards,
        questCompleted;

    [SerializeField]
    private Quest currentQuest;

    // Start is called before the first frame update
    void Start()
    {
        questScreen.SetActive(false);
        questDescriptor.SetActive(false);
        activeQuestList = new List<Quest>();
        slotsList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() { 
        questCount = activeQuestList.Count;
    }

    public void AddQuest(QuestStructure quest, int currentStep, NPCData npc)
    {
        Quest _quest = new Quest(quest, currentStep);
        _quest.SetNPC(npc);
        activeQuestList.Add(_quest);
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

    public void OpenDescription(Quest quest)
    {
        currentQuest = quest;
        questTitle.text = quest.data.questData.title;
        questDescriotion.text = quest.data.questData.description;
        questGoals.text = quest.data.GetQuestGoals();
        questRewards.text = quest.data.GetQuestRewardsDescription();
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
        if(quest.data.questData.isTutorial){
           StartCoroutine(ResponseDialog());
        }
        activeQuestList.Clear();
    }

    public void OpenScreen()
    {
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
