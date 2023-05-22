using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float health = 100;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement playerMovement; 
    private bool isDead; 

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isDead = false;
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
        // Destroy(this.gameObject);
    }

    public bool IsDead(){  return isDead; }
}
