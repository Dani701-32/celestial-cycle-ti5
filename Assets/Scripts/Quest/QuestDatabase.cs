using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Database", menuName = "Save System/QuestDatabase")]
public class QuestDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public QuestStructure[] Items;
    public Dictionary<QuestStructure, int> GetId = new Dictionary<QuestStructure, int>();
    public Dictionary<int, QuestStructure> GetItem = new Dictionary<int, QuestStructure>();
    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<QuestStructure, int>();
        GetItem = new Dictionary<int, QuestStructure>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize() { }
}
