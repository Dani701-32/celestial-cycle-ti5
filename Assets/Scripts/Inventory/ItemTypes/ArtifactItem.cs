using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItem : InventoryItem
{
    private GameController controller;
    public ArtifactItem(InventoryItemData data)
        : base(data) {

        controller = GameController.gameController;
         }

    public override void Use()
    {
        controller.player.EquipeArtifact(this.data.prefab);
        controller.player.artifactSprite.sprite = data.icon;
        controller.player.artifactSprite.enabled = true;
    }
    public override void Remove()
    {
        controller.player.RemoveArtifact();
        controller.player.artifactSprite.sprite = null;
        controller.player.artifactSprite.enabled = false;
    }
}
