using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private GameController gameController;

    public bool isSave, isActive;

    [SerializeField]
    private GameObject canvas, popUp;

    void Start()
    {
        gameController = GameController.gameController; 
        canvas.SetActive(false);
        isActive = false;
        popUp.SetActive(false);
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.interactAction.triggered && !isActive)
        {
            CallSaveUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            isSave = true;
            canvas.SetActive(true);
            this.playerMovement = playerMovement;          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSave = false;
            canvas.SetActive(false);
            playerMovement = null; 
        }
    }

    public void CallSaveUI()
    {
        // savingLoading.Save();
        popUp.SetActive(true);
        isActive = true;
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameController.StopCamera();
    }

    public void DisableScreen(){
        popUp.SetActive(false);
        isActive = false;
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameController.ReleaseCamera();
        Time.timeScale = 1.0f;
    }

}
