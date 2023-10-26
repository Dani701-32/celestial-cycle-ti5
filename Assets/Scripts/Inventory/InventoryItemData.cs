using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    [HideInInspector] public int saveID;

    [Header("Dados Genericos")]
    public ItemType type;
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
    public Sprite iconType;
    public GameObject prefab;
    public MoonPhases aspect;
    [Header("Consumiveis e Coletáveis")]
    public int maxStack;
    public bool canStack;

}

public enum ItemType
{
    Collectable,
    Weapon,
    Artifact,
    Consumable,
}
