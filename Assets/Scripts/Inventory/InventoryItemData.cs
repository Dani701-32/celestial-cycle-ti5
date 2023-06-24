using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    [Header("Dados Genericos")]
    public ItemType type;
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
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
