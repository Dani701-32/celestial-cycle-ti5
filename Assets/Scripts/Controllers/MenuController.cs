using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        loadButton.SetActive(hasSaveGame);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGame()
    {
        Debug.Log("Carregar jogo");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
