using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public bool isSaved;
    public GameSaveManager gameSaveManager;
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && isSaved)
        {
            gameSaveManager.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.K) && isSaved)
        {
            gameSaveManager.LoadGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isSaved = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isSaved = false;
    }
}
