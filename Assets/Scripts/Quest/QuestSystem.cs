using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        questScreen.SetActive(false);
        questDescriptor.SetActive(false);
        activeQuestList = new List<Quest>();
        slotsList = new List<GameObject>();
        trakedQuestScreen.SetActive(false);
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
        TrackQuest();
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
            questTrakerGoals.text = $"Concluído \nRetorne para {trakedQuest.currentNPC.dataNPC.Name}";
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
