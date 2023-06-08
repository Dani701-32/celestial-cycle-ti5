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
    public float health = 100;

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

    public bool hasArtifact { get; private set; }
    public bool hasWeapon { get; private set; }

    //public string SavePlayerData()
    //{
    //    return health.ToString();
    //}

    //public string ReceivePlayerData(string data)
    //{
    //    health = float.Parse(data);

    //    return health.ToString();
    //}

    void Start()
    {
        controller = GameController.gameController;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        inventoryAction = playerMovement.playerInput.actions["Inventory"];
        animator = GetComponent<Animator>();
        isDead = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Carrega as propriedades do Player
        //controller.LoadPlayer(health, this.transform.position, this.transform);
    }

    private void Update()
    {
        if (inventoryAction.triggered)
        {
            controller.MenuScreen();
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
