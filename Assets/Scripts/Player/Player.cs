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
    private bool isDead;
    InputAction inventoryAction;

    [Header("HUD")]
    public Image weaponSprite;
    public Image artifactSprite;

    void Start()
    {
        controller = GameController.gameController;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        inventoryAction = playerMovement.playerInput.actions["Inventory"];
        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            controller.InventoryScene();
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
        GameObject artifact = Instantiate(prefab, artifactSpot);
        playerMovement.currentArtifact = artifact;
    }

    public void EquipWeapon(GameObject prefab)
    {
        GameObject weapon = Instantiate(prefab, weaponSpot);
        playerMovement.currentWeapon = weapon;
        weapon.SetActive(playerMovement.combatMode);
    }

    public void RemoveWeapon()
    {
        Debug.Log("Remvoer arma");
        playerMovement.UniqueppedWeapon();
    }

    public void RemoveArtifact()
    {
        Debug.Log("Remvoer artefato");
        Destroy(playerMovement.currentArtifact);
        playerMovement.currentArtifact = null;
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
