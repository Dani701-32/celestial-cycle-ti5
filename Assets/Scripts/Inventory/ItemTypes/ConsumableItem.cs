using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : InventoryItem
{
    private Consumable consumable;

    public ConsumableItem(InventoryItemData data)
        : base(data)
    {
        consumable = data.prefab.GetComponent<Consumable>();
    }

    public override void Use()
    {
        consumable.Use();
        GameController.gameController.inventorySystem.Remove(data);
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
