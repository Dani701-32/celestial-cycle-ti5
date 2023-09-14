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

    public bool Use(int index)
    {
        if (controller.player.artifactsRoster[index] != null)
        {
            Debug.Log("Slot ocupado");
            return false;
        }
        Debug.Log("Slot Livre");
        controller.player.AddArtifactRoster(this, index);
        return true;

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

    public override void Remove() { }

    public void Remove(int index)
    {
        Debug.Log("Desequipar");
        equiped = false;
        controller.player.RemoveArtifact(index);

    }
}
