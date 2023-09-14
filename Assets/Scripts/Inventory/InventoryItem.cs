using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }
    public bool equiped;
    public InventoryItem(InventoryItemData data)
    {
        this.data = data;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public abstract void Use();
    public abstract void Remove();
}
