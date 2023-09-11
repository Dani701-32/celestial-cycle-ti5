using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, ISaveable
{
    private GameController controller;
    private SavingLoading savingLoading;
    public bool isDamage;

    [SerializeField]
    float health = 50;
    public float maxHealth { get; private set; }

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
    public Artifact[] artifactsRoster = new Artifact[4];
    public Artifact currentArtifact;
    private bool isDead;
    InputAction inventoryAction;
    InputAction artifactsAction;
    InputAction changeArtifact;
    InputBinding firstBinding,
        secondBinding,
        thirdBinding,
        fourthBinding;

    [Header("HUD")]
    public Image weaponSprite;
    public Image artifactSprite;
    public GameObject artifactSlider;

    [Header("Sliders")]
    public Slider fullMoonSlider;
    public Slider waxingMoonSlider;
    public Slider waningMoonSlider;
    public Slider newMoonSlider;
    public Slider lifeSlider;

    [SerializeField]
    private Slider slider;
    public bool hasArtifact { get; private set; }
    public bool hasWeapon { get; private set; }

    private Vector3 playerPos;

    //Controla se as quests estão abertas
    public bool QuestIsOpen;

    public void InitializeVariables()
    {
        playerPos = this.transform.position;
    }

    void Start()
    {
        // savingLoading = FindObjectOfType<SavingLoading>().GetComponent<SavingLoading>();
        controller = GameController.gameController;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        inventoryAction = playerMovement.playerInput.actions["Inventory"];
        artifactsAction = playerMovement.playerInput.actions["Artifact"];

        changeArtifact = playerMovement.playerInput.actions["changeArtifact"];
        firstBinding = changeArtifact.bindings[0];
        secondBinding = changeArtifact.bindings[1];
        thirdBinding = changeArtifact.bindings[2];
        fourthBinding = changeArtifact.bindings[3];

        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
        artifactSlider.SetActive(false);

        fullMoonSlider.maxValue = maxFullMoon;
        fullMoonSlider.value = currentFullMoon;
        waxingMoonSlider.maxValue = maxFullMoon;
        waxingMoonSlider.value = currentFullMoon;
        waningMoonSlider.maxValue = maxFullMoon;
        waningMoonSlider.value = currentFullMoon;
        newMoonSlider.maxValue = maxNewMoon;
        newMoonSlider.value = currentNewMoon;

        lifeSlider.maxValue = maxHealth;
        lifeSlider.value = health;

        fullMoonSlider.gameObject.SetActive(false);
        waxingMoonSlider.gameObject.SetActive(false);
        waningMoonSlider.gameObject.SetActive(false);
        newMoonSlider.gameObject.SetActive(false);

        QuestIsOpen = false;
        maxHealth = 100f;
        // if (!savingLoading.StatusFile())
        // {
        //     InitializeVariables();
        //     Debug.Log("Inicializou o Sistema de Dia e Noite mas n�o tem save");
        // }
    }

    private void Update()
    {
        playerPos = this.transform.position;

        if (inventoryAction.triggered)
        {
            if (QuestIsOpen)
            {
                controller._NPCDialogue.CloseScreen();
            }
            else
            {
                controller.MenuScreen();
            }
        }
        if (currentArtifact != null)
        {
            if (artifactsAction.triggered)
            {
                currentArtifact.Use();
            }
            UpdateArtifact();
        }
        if (changeArtifact.triggered)
        {
            ChangeArtifact();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDamage)
        {
            health -= damage;
            animator.SetTrigger("damage");
            lifeSlider.value = health;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

    public void EquipeArtifact(GameObject prefab)
    {
        if (hasArtifact)
        {
            RemoveArtifact();
        }
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

    public void ChangeArtifact()
    {
        Debug.Log(changeArtifact.name  );
    }

    public bool IsDead()
    {
        return isDead;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            s_xPos = playerPos.x,
            s_yPos = playerPos.y,
            s_zPos = playerPos.z
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        playerPos.x = saveData.s_xPos;
        playerPos.y = saveData.s_yPos;
        playerPos.z = saveData.s_zPos;
    }

    [Serializable]
    private struct SaveData
    {
        public float s_xPos;
        public float s_yPos;
        public float s_zPos;
    }
}
