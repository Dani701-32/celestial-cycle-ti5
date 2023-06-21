using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameController controller;

    [SerializeField]
    float health = 100;

    [SerializeField]
    Animator animator;
    public float maxFullMoon,
        maxWaxingMoon,
        maxWaningMoon,
        maxNewMoon;
    public float currentFullMoon,
        currentWaxingMoon,
        currentWaningMoon,
        currentNewMoon;
    public Transform artifactSpot;
    public Transform weaponSpot;
    public PlayerMovement playerMovement { get; private set; }
    public Artifact currentArtifact;
    private bool isDead;
    InputAction inventoryAction;
    InputAction artifactsAction;

    [Header("HUD")]
    public Image weaponSprite;
    public Image artifactSprite;
    public GameObject artifactSlider;

    [Header("Sliders")]
    public Slider fullMoonSlider;
    public Slider waxingMoonSlider;
    public Slider waningMoonSlider;
    public Slider newMoonSlider;

    [SerializeField]
    private Slider slider;
    public bool hasArtifact { get; private set; }
    public bool hasWeapon { get; private set; }

    void Start()
    {
        controller = GameController.gameController;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        inventoryAction = playerMovement.playerInput.actions["Inventory"];
        artifactsAction = playerMovement.playerInput.actions["Artifact"];
        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
        artifactSlider.SetActive(false);

        fullMoonSlider.maxValue = maxFullMoon;
        fullMoonSlider.value = currentFullMoon;
        newMoonSlider.maxValue = maxNewMoon;
        newMoonSlider.value = currentNewMoon;

        fullMoonSlider.gameObject.SetActive(false);
        newMoonSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            controller.MenuScreen();
        }
        if (currentArtifact != null)
        {
            if (artifactsAction.triggered)
            {
                currentArtifact.Use();
            }
            UpdateArtifact();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    public void EquipeArtifact(GameObject prefab)
    {
        hasArtifact = true;
        GameObject artifact = Instantiate(prefab, artifactSpot);
        playerMovement.currentArtifact = artifact;
        if (artifact.TryGetComponent(out Artifact component))
        {
            currentArtifact = component;
            ArtifactEnergy(currentArtifact.artifactMoon, true);
            artifactSlider.SetActive(true);
            slider = artifactSlider.GetComponent<Slider>();
            slider.maxValue = currentArtifact.cooldown;
        }
    }

    public void EquipWeapon(GameObject prefab)
    {
        hasWeapon = true;
        GameObject weapon = Instantiate(prefab, weaponSpot);
        playerMovement.currentWeapon = weapon;
        weapon.SetActive(playerMovement.combatMode);
    }

    public void RemoveWeapon()
    {
        hasWeapon = false;
        Debug.Log("Remvoer arma");
        playerMovement.UniqueppedWeapon();
    }

    public void RemoveArtifact()
    {
        hasArtifact = false;
        Debug.Log("Remvoer artefato");
        Destroy(playerMovement.currentArtifact);
        playerMovement.currentArtifact = null;
        ArtifactEnergy(currentArtifact.artifactMoon, false);
        currentArtifact = null;
        artifactSlider.SetActive(false);

    }

    public void UpdateHud()
    {
        if (currentFullMoon > maxFullMoon)
            currentFullMoon = maxFullMoon;
        if (currentNewMoon > maxNewMoon)
            currentNewMoon = maxNewMoon;
        fullMoonSlider.value = currentFullMoon;
        newMoonSlider.value = currentNewMoon;
    }

    private void UpdateArtifact()
    {
        slider.value = currentArtifact.remaningCooldown;
        fullMoonSlider.value = currentFullMoon;
        newMoonSlider.value = currentNewMoon;
    }

    private void ArtifactEnergy(MoonPhases moonType, bool activated)
    {
        switch (moonType)
        {
            case MoonPhases.FirstQuarter:
            case MoonPhases.FullMoon:
                fullMoonSlider.gameObject.SetActive(activated);
                break;
            case MoonPhases.ThirdQuarter:
            case MoonPhases.NewMoon:
                newMoonSlider.gameObject.SetActive(activated);
                break;
        }
    }

    void Die()
    {
        playerMovement.enabled = false;
        animator.SetTrigger("death");
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        GameController.gameController.DeathScreen();
    }

    public bool IsDead()
    {
        return isDead;
    }
}
