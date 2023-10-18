using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Save System/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public InventoryItemData[] Items;
    public Dictionary<InventoryItemData, int> GetId = new Dictionary<InventoryItemData, int>();
    public Dictionary<int, InventoryItemData> GetItem = new Dictionary<int, InventoryItemData>();
    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<InventoryItemData, int>();
        GetItem = new Dictionary<int, InventoryItemData>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize() { }
}
