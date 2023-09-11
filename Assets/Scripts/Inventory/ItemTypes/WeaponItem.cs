using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : InventoryItem
{
    private GameController controller;
    public WeaponItem(InventoryItemData data)
        : base(data) { 
            controller = GameController.gameController;
        }

    public override void Use()
    {
        if(controller.player.hasWeapon){
            Debug.Log("Slot Ocupado");
            return;
        }
        equiped = true;
        controller.player.EquipWeapon(this.data.prefab);
        controller.player.weaponSprite.sprite = data.icon;
        controller.player.weaponSprite.enabled = true;
    }
    public override void Use(int index){}
    
    public override void Remove()
    {
        equiped = false;
        controller.player.RemoveWeapon();
        controller.player.weaponSprite.sprite = null;
        controller.player.weaponSprite.enabled = false;
    }
}
