using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/New Quest", order = 0)]
public class QuestStructure : ScriptableObject
{
    [System.Serializable]
    public struct QuestData
    {
        public string title;
        public string description;

        public string questID;

        [Header("Tutorial")]
        public bool isTutorial;
    }

    public abstract class QuestReward : ScriptableObject
    {
        [SerializeField]
        protected string rewardDescription;

        [SerializeField]
        protected InventoryItemData itemDataReference;
        public string RewardName { get; private set; }
        public Sprite Sprite { get; private set; }

        public void Invoke()
        {
            RewardName = itemDataReference.displayName;
            Sprite = itemDataReference.icon;
        }

        public abstract string GetDescription();
        public abstract void GetReward();
    }

    public abstract class QuestGoal : ScriptableObject
    {
        [SerializeField]
        protected string goalDescription;
        public int currentAmount = 0;
        public int minimumAmount = 1;
        public bool isGoalCompleted = false;

        public void Invoke()
        {
            currentAmount = 0;
            isGoalCompleted = false;
        }

        public virtual string GetDescription()
        {
            return goalDescription;
        }

        public abstract void isCompleted();
        public abstract void Complete();
    }

    //Armazenamento dos dados da quest
    [Header("Quest Data")]
    public QuestData questData;

    [Header("Quest Goals")]
    public List<QuestGoal> goals;

    [Header("Quest Reward")]
    public List<QuestReward> rewards;
    public bool isQuestCompleted;

    public void Invoke()
    {
        isQuestCompleted = false;
        Debug.Log("Quest restarted");
        foreach (QuestGoal questGoal in goals)
        {
            questGoal.Invoke();
        }
        foreach (QuestReward reward in rewards)
        {
            reward.Invoke();
        }
    }

    public void CompleteQuest()
    {
        foreach (QuestGoal questGoal in goals)
        {
            questGoal.isCompleted();
            if (!questGoal.isGoalCompleted)
                return;
        }
        isQuestCompleted = true;
    }

    public string GetQuestGoals()
    {
        string textGoals = "";
        foreach (QuestGoal goal in goals)
        {
            textGoals += $"{goal.currentAmount}/{goal.minimumAmount} - {goal.GetDescription()}\n";
        }
        return textGoals;
    }

    public string GetQuestRewardsDescription()
    {
        string textReward = "";
        foreach (QuestReward reward in rewards)
        {
            textReward += $"{reward.GetDescription()}\n";
        }
        return textReward;
    }

    public void GetRewards()
    {
        foreach (QuestGoal goal in goals)
        {
            goal.Complete();
        }
        Debug.Log("GetRewards");
        foreach (QuestReward reward in rewards)
        {
            reward.GetReward();
        }
    }

    [Header("Dialogues for Quest")]
    public int aceptedMessage,
        declineMessage;
    public string[] NPCsmessages;
}
