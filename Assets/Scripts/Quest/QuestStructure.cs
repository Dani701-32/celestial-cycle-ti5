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
    }

    public abstract class QuestReward : ScriptableObject
    {
        [SerializeField]
        protected string rewardDescription;

        public abstract string GetDescription();

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
            isGoalCompleted = true;
        }

        public virtual string GetDescription()
        {
            return goalDescription;
        }

        public abstract void isCompleted();
    }

    //Armazenamento dos dados da quest
    [Header("Quest Data")]
    public QuestData questData;

    [Header("Quest Goals")]
    public List<QuestGoal> goals;

    [Header("Quest Reward")]
    public List<QuestReward> rewards;

    public bool isQuestCompleted = false;

    public void Invoke()
    {
        isQuestCompleted = false;
        foreach (QuestGoal questGoal in goals)
        {
            questGoal.Invoke();
        }
    }

    public void CompleteQuest()
    {
        foreach (QuestGoal questGoal in goals)
        {
            if (questGoal.isGoalCompleted)
                return;
        }
        isQuestCompleted = true;
    }

    public string GetQuestGoals()
    {
        string textGoals = "Objetivos:\n";
        foreach (QuestGoal goal in goals)
        {
            textGoals += $"{goal.currentAmount}/{goal.minimumAmount} - {goal.GetDescription()}\n";
        }
        return textGoals;
    }

    public string GetQuestRewards() {
        string textReward = "Recompensas:\n";
        foreach (QuestReward reward in rewards){
            textReward+= $"{reward.GetDescription()}\n";
        }
        return textReward;
     }

    public string[] NPCsmessages;
}
