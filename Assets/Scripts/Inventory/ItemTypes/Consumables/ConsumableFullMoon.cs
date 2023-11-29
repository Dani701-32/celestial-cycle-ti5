using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableFullMoon : Consumable
{
    public float lifeRestored; 
    public override void Use()
    {
        Player player = GameController.gameController.player;
        player.RestoreLife(lifeRestored);

    }
}
