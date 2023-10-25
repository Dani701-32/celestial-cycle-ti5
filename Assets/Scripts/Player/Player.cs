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
    private int numArtifacts = 4;
    public bool isDamage;

    [SerializeField]
    float health = 50;
    public float maxHealth;

    [SerializeField]
    Animator animator;
    public int maxEnergy;
    public float currentFullMoon,
        currentWaxingMoon,
        currentWaningMoon,
        currentNewMoon;
    public Transform artifactSpot;
    public Transform weaponSpot;
    public PlayerMovement playerMovement { get; private set; }
    public GameObject[] artifactsRoster = new GameObject[4];
    public Artifact currentArtifact;
    private bool isDead;
    InputAction inventoryAction;
    InputAction artifactsAction;
    InputAction firstArtifactAction,
        secondArtifactAction,
        thirdArtifactAction,
        fourthArtifactAction;

    InputAction[] artifactActions;

    [Header("HUD")]
    public Image weaponSprite;
    public Image artifactSprite;
    public GameObject artifactSlider;
    public GameObject artifactButton;
    public GameObject weaponButton;

    [Header("Sliders")]
    public ArtifactUIController[] moonArtifacts = new ArtifactUIController[4];

    public Slider lifeSlider;

    [SerializeField]
    private Slider slider;
    public bool hasArtifact;
    public bool hasWeapon { get; private set; }

    private Vector3 playerPos;

    //Controla se as quests estão abertas
    public bool QuestIsOpen;

    private int previusIndex = 99;

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

        artifactActions = new InputAction[]{
           playerMovement.playerInput.actions["FirstArtifact"],
             playerMovement.playerInput.actions["SecondArtifact"],
            playerMovement.playerInput.actions["ThirdArtifact"],
            playerMovement.playerInput.actions["FourthArtifact"],
        };

        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
        artifactSlider.SetActive(false);
        // artifactButton.SetActive(false);
        // weaponButton.SetActive(false);


        lifeSlider.maxValue = maxHealth;
        lifeSlider.value = health;

        QuestIsOpen = false;
        maxHealth = 100f;
        if (moonArtifacts.Length > 0)
        {
            foreach (ArtifactUIController slider in moonArtifacts)
            {
                slider.StartSlider(maxEnergy);
            }
        }

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
        ChangeArtifact();
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

    public void EquipeArtifact(int index)
    {
        if (hasArtifact)
        {
            ChangeArtifactEquipped(index);
        }
        hasArtifact = true;
        GameObject artifact = Instantiate(artifactsRoster[index], artifactSpot);
        playerMovement.currentArtifact = artifact;
        playerMovement.currentArtifact.SetActive(false);
        if (artifact.TryGetComponent(out Artifact component))
        {
            currentArtifact = component;
            artifactSprite.enabled = true;
            artifactSprite.sprite = moonArtifacts[index].image.sprite;
            artifactSlider.SetActive(true);
            slider = artifactSlider.GetComponent<Slider>();
            slider.maxValue = currentArtifact.cooldown;
        }
        previusIndex = index;
    }

    public void EquipWeapon(GameObject prefab)
    {
        hasWeapon = true;
        GameObject weapon = Instantiate(prefab, weaponSpot);
        playerMovement.currentWeapon = weapon;
        weapon.SetActive(playerMovement.combatMode);
        // weaponButton.SetActive(playerMovement.combatMode);
    }

    public void RemoveWeapon()
    {
        hasWeapon = false;
        Debug.Log("Remvoer arma");
        playerMovement.UniqueppedWeapon();
    }

    public void RemoveArtifact(int index)
    {
        if (artifactsRoster[index] == null) return;

        if (artifactsRoster[index].GetComponent<Artifact>() != null && playerMovement.currentArtifact != null)
        {
            if (artifactsRoster[index].GetComponent<Artifact>().id == playerMovement.currentArtifact.GetComponent<Artifact>().id)
            {
                hasArtifact = false;
                Destroy(playerMovement.currentArtifact);
                playerMovement.currentArtifact = null;
                currentArtifact = null;
                artifactSlider.SetActive(false);
                artifactSprite.enabled = false;
            }
        }

        artifactsRoster[index] = null;
        moonArtifacts[index].CloseSlider();
    }
    public void ChangeArtifactEquipped(int index)
    {
        if(previusIndex > 4) return;
        if (artifactsRoster[previusIndex].GetComponent<Artifact>().id == playerMovement.currentArtifact.GetComponent<Artifact>().id)
        {
            Debug.Log("Remover artefato");
            Destroy(playerMovement.currentArtifact.gameObject);
            playerMovement.currentArtifact = null;
            currentArtifact = null;
            artifactSlider.SetActive(false);
            artifactSprite.enabled = false;
        }
    }

    private void UpdateArtifact()
    {
        slider.value = currentArtifact.remaningCooldown;
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
        for (int i = 0; i < artifactActions.Length; i++)
        {
            if (artifactActions[i].triggered)
            {
                Debug.Log("Teste " + (i + 1));
                if (artifactsRoster[i] != null)
                {
                    EquipeArtifact(i);
                } 
                break; // Saia do loop após encontrar a primeira ação acionada.
            }
        }
        
    }
    public void AddArtifactRoster(ArtifactItem artefactItem, int index)
    {
        GameObject prefab = artefactItem.data.prefab;
        artifactsRoster[index] = prefab;
        Artifact component = prefab.GetComponent<Artifact>();
        moonArtifacts[index].OpenSlider(artefactItem.data.icon, artefactItem.data.iconType);
    }
    public bool HasArtifactRoster(GameObject prefab)
    {
        foreach (GameObject item in artifactsRoster)
        {
            if (item == prefab)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasArtifactRoster(int index)
    {
        return artifactsRoster[index] != null;
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
