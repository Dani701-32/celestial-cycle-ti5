using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    private List<Quest> activeQuestList;

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
    void Update() { }

    public void AddQuest(QuestStructure quest, int currentStep)
    {
        activeQuestList.Add(new Quest(quest, currentStep));
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
        questRewards.text = quest.data.GetQuestRewards();
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

    private void CheckQuests()
    {
        if (activeQuestList.Count == 0)
            return;

        foreach (Quest item in activeQuestList)
        {
            Debug.Log("Teste quest");
            item.data.CompleteQuest();
            if (item.data.isQuestCompleted)
            {
                questCompleted.text = "Quest Completed";
            }
            else
            {
                questCompleted.text = "";
            }
        }
    }

    public void OpenScreen()
    {
        UpdateScreen();
        CheckQuests();
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
}
