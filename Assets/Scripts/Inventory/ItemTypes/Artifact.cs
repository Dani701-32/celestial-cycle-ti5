using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : MonoBehaviour
{
    [Header("Main Controllers")]
    public MoonPhases artifactMoon;
    public float cost;
    
    public float cooldown = 5f;
    public float remaningCooldown = 0;

    protected bool useArtifact = false;
    protected GameController gameController;
    public GameObject particle;
    public List<GameObject> hasAffacted;
    public abstract void Use();

    private void Start()
    {
        gameController = GameController.gameController;
        useArtifact = false;
        // charge = maxCharge;
    }
}
