using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : InventoryItem
{
  public CollectableItem(InventoryItemData data): base(data){

  }
    public override void Use()
    {
        Debug.Log("Usou Coletavel");
    }
    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
