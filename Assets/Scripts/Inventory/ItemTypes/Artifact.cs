using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : MonoBehaviour
{
    [Header("Main Controllers")]
    public MoonPhases artifactMoon;
    public float charge, maxCharge, cost;
    protected bool useArtifact = false;
    protected GameController gameController;
    public GameObject particle;
    public List<GameObject> hasAffacted;
    public abstract void Use();
    public abstract void Recharge();

    private void Start()
    {
        gameController = GameController.gameController;
        useArtifact = false;
        // charge = maxCharge;
    }
}
