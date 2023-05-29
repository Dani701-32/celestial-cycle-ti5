using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(menuName ="Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public int maxStack;
    public bool canStack;
}
