using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SavingLoading savingLoading;

    public bool isSave;

    [SerializeField]
    private GameObject canvas;

    void Start()
    {
        savingLoading = SavingLoading.instance;
        canvas.SetActive(false);
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.interactAction.triggered)
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
            this.playerMovement = null;  
        }
    }

    public void CallSaveUI()
    {
        savingLoading.Save();
    }

}
