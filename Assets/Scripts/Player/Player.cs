using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float health = 100;

    [SerializeField]
    Animator animator;

    [SerializeField]
    PlayerMovement playerMovement;
    private bool isDead;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
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
