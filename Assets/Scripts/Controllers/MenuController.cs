using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGameLevel, newGameLevelCheat;
    private string levelToLoad;
    public static MenuController instance;

    [SerializeField]
    private GameObject loadButton;
    private SavingLoading savingLoading;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
 
        savingLoading = FindObjectOfType<SavingLoading>().GetComponent<SavingLoading>();
    }

    void Start()
    {
        loadButton.SetActive(savingLoading.StatusFile());
    }

    public void NewGame()
    {
        Debug.Log("Iniciou um Novo Jogo");
        SceneManager.LoadScene(newGameLevel);
    }

    public void NewGameCheat()
    {
        SceneManager.LoadScene(newGameLevelCheat);
    }

    public void LoadGame()
    {
        Debug.Log("Carregou o jogo a partir do Save");
        savingLoading.loadByMenu = true;
        SceneManager.LoadScene(newGameLevel);

    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.K)) SceneManager.LoadScene(newGameLevel);
    //}

    public void Exit()
    {
        Application.Quit();
    }
}
