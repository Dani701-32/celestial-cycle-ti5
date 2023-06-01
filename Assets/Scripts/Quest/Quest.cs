using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestStructure data { get; private set; }
    public int currentIndex { get; private set; }
    public bool questCompleted { get; private set; }

    public Quest(QuestStructure data, int currentIndex)
    {
        this.data = data;
        this.currentIndex = currentIndex;
    }

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

    public string StatusQuest(bool isAcepted)
    {
        if (isAcepted)
        {
            currentIndex = this.data.aceptedMessage;
            ProgressQuest();
        }
        else
        {
            currentIndex = 0;
        }
        return this.data.NPCsmessages[
            (isAcepted) ? this.data.aceptedMessage : this.data.declineMessage
        ];
    }
}
