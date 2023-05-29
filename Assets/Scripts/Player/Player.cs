using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameController controller;
    [SerializeField]
    float health = 100;

    [SerializeField]
    Animator animator;

    [SerializeField]
    PlayerMovement playerMovement;
    private bool isDead;
    InputAction inventoryAction;

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
    private void Update() {
        if(inventoryAction.triggered){
            
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
