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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
