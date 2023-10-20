using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGameLevelCheat;
    public string levelToLoad;
    public static MenuController instance;

    public bool hasSaveGame;

    [SerializeField]
    private GameObject loadButton;
    private SavingLoading savingLoading;

    public GameObject canvasGame;

    public void CallMenu()
    {
        hasSaveGame = false;
        GameController.gameController.isMenu = false;
        loadButton.SetActive(GameController.gameController.savingLoadingController.StatusFile());
        canvasGame.SetActive(false);
        GameController.gameController.MenuScreen();
    }
        

    void Start()
    {
        CallMenu();
    }

    public void NewGame()
    {
        hasSaveGame = false;
        GameController.gameController.isMenu = true;
        GameController.gameController.MenuScreen();
        GameController.gameController.SwitchToCameraFreeLook(GameController.gameController.freelookCamera);
        canvasGame.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void NewGameCheat()
    {
        hasSaveGame = false;
        SceneManager.LoadScene(newGameLevelCheat);
    }

    public void LoadGame()
    {
        hasSaveGame = true;
        canvasGame.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
