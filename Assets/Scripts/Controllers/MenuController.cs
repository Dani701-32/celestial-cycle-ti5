using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGameLevel;
    private string levelToLoad;
    public static MenuController instance;

    [SerializeField]
    private GameObject loadButton;
    private bool hasSaveGame = false;
    private SavingLoading savingLoading;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //Sistema de save incompleto. 
        // savingLoading = FindObjectOfType<SavingLoading>().GetComponent<SavingLoading>();
    }

    void Start()
    {
        // hasSaveGame = savingLoading.StatusFile();
        // loadButton.SetActive(hasSaveGame);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(newGameLevel);
        Debug.Log("Carregar jogo");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
