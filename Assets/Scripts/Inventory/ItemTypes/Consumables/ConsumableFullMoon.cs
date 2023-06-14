using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableFullMoon : Consumable
{
    public override void Use()
    {
        Player player = GameController.gameController.player;
        player.currentFullMoon += charge;
        player.UpdateHud();
    }
}
