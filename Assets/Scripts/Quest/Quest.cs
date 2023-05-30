using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestStructure data { get; private set; }
    public int currentIndex { get; private set; }
    public bool questCompleted { get; private set; }

    public Quest(QuestStructure data)
    {
        this.data = data;
        currentIndex = 0;
    }

    public void ProgressQuest()
    {
        currentIndex++;
    }

    public void CompleteQuest()
    {
        currentIndex = 5;
        data.CompleteQuest();
        questCompleted = data.isQuestCompleted;
    }
    public string CurrentDialogue()
    {
        return data.NPCsmessages[currentIndex];
    }
}
