using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Save System/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public InventoryItemData[] Items;
    public Dictionary<InventoryItemData, string> GetId = new Dictionary<InventoryItemData, string>();
    public Dictionary<string, InventoryItemData> GetItem = new Dictionary<string, InventoryItemData>();
    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<InventoryItemData, string>();
        GetItem = new Dictionary<string, InventoryItemData>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i.ToString());
            GetItem.Add(i.ToString(), Items[i]);
        }
    }

    public void OnBeforeSerialize() { }
}
