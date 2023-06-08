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
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            controller.MenuScreen();
        }
        if (artifactsAction.triggered && currentArtifact != null)
        {
            currentArtifact.Use();
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
        currentArtifact = null;
    }

    void Die()
    {
        playerMovement.enabled = false;
        animator.SetTrigger("death");
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        GameController.gameController.DeathScreen();
        // Destroy(this.gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
