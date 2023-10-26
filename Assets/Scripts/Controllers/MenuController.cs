using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels:")]
    public string levelToLoad;
    public string newGameLevelCheat;  
    public static MenuController instance;
    public bool hasSaveGame;

    [Header("UI:")]
    public GameObject loadButton;

    public GameObject canvasGame, uiDayNight;
    [SerializeField]
    public GameObject popNewGame, popLoadGame, mainMenuBackground;

    public void CheckSave(bool status)
    {
        loadButton.SetActive(status);
    }

    public void CallMenu()
    {
        hasSaveGame = false;
        GameController.gameController.isMenu = false;
        CheckSave(GameController.gameController.savingLoadingController.StatusFile());
        canvasGame.SetActive(false);
        uiDayNight.SetActive(false);
        GameController.gameController.MenuScreen();
    }

    public void BaseContextMenu()
    {
        GameController.gameController.isMenu = true;
        GameController.gameController.MenuScreen();
        GameController.gameController.inventorySystem.StartInventory();
        GameController.gameController.SwitchToCameraFreeLook(GameController.gameController.freelookCamera);
        canvasGame.SetActive(true);
        uiDayNight.SetActive(true);
        popNewGame.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
        

    void Start()
    {
        CallMenu();
    }

    public void NewGame()
    {
        hasSaveGame = false;
        GameController.gameController.player.PlayerStartPosition();
        GameController.gameController.dayNightController.StartDayNightSystem(); 
        BaseContextMenu();   
    }

    public void LoadGame()
    {
        hasSaveGame = true;       
        GameController.gameController.savingLoadingController.CallSave();
        GameController.gameController.dayNightController.StartDayNightSystem();
        BaseContextMenu();        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
