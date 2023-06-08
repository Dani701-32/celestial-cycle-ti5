using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItem : InventoryItem
{
    private GameController controller;
    private float charge;

    public ArtifactItem(InventoryItemData data)
        : base(data)
    {
        controller = GameController.gameController;
        charge = this.data.prefab.GetComponent<Artifact>().charge;
    }

    public override void Use()
    {
        if (controller.player.hasArtifact)
        {
            Debug.Log("Slot Ocupado");
            return;
        }
        equiped = true;
        Debug.Log("Curren charge " + charge);
        controller.player.EquipeArtifact(this.data.prefab, charge);
        controller.player.artifactSprite.sprite = data.icon;
        controller.player.artifactSprite.enabled = true;
    }

    public override void Remove()
    {
        Debug.Log("Desequipar");
        charge = controller.player.currentArtifact.charge;
        equiped = false;
        controller.player.RemoveArtifact();
        controller.player.artifactSprite.sprite = null;
        controller.player.artifactSprite.enabled = false;
    }
}
