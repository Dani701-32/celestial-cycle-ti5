using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItem : InventoryItem
{
    private GameController controller;

    public ArtifactItem(InventoryItemData data)
        : base(data)
    {
        controller = GameController.gameController;
    }

    public override void Use() { }

    public override void Use(int index)
    {
        if (controller.player.artifactsRoster[index] != null)
        {
            Debug.Log("Slot ocupado");
            return;
        }
        Debug.Log("Slot Livre");
        controller.player.artifactsRoster[index] = data.prefab;

        // if (controller.player.hasArtifact)
        // {
        //     Debug.Log("Slot Ocupado");
        //     return;
        // }
        // equiped = true;
        // controller.player.EquipeArtifact(this.data.prefab);
        // controller.player.artifactSprite.sprite = data.icon;
        // controller.player.artifactSprite.enabled = true;
    }

    public override void Remove()
    {
        Debug.Log("Desequipar");
        equiped = false;
        controller.player.RemoveArtifact();
        controller.player.artifactSprite.sprite = null;
        controller.player.artifactSprite.enabled = false;
    }
}
