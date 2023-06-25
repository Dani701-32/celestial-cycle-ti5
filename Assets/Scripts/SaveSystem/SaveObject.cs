using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    private SavingLoading savingLoading;

    [SerializeField]
    private bool isSave;

    public GameObject messageObject;

    void Awake()
    {
        // savingLoading = FindObjectOfType<SavingLoading>().GetComponent<SavingLoading>();
        ControlStateMessage(messageObject, false);
    }

    public void ControlStateMessage(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSave = true;
            ControlPlayerActionsOnSave(isSave);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isSave = false;
        ControlPlayerActionsOnSave(isSave);
    }

    public void CallSaveUI()
    {
        savingLoading.Save();
        isSave = false;
        ControlPlayerActionsOnSave(isSave);
    }

    public void ExitSaveUI()
    {
        isSave = false;
        ControlPlayerActionsOnSave(isSave);
    }

    public void ControlPlayerActionsOnSave(bool state)
    {
        if(state)
        {
            ControlStateMessage(messageObject, true);
            Cursor.lockState = CursorLockMode.None;
            GameController.gameController.player.playerMovement.enabled = false;
            GameController.gameController.StopCamera();
        }
        else
        {
            ControlStateMessage(messageObject, false);
            Cursor.lockState = CursorLockMode.Locked;
            GameController.gameController.ReleaseCamera();
            GameController.gameController.player.playerMovement.enabled = true;
        }
    }
}
