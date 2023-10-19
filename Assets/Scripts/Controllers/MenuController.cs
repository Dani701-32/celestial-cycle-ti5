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
    public bool byLoad;

    [SerializeField]
    private GameObject loadButton;
    private SavingLoading savingLoading;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        //DontDestroyOnLoad(this);

        //savingLoading = FindObjectOfType<SavingLoading>().GetComponent<SavingLoading>();
    }

    void Start()
    {
        //loadButton.SetActive(savingLoading.StatusFile());
        loadButton.SetActive(true);
    }

    public void NewGame()
    {
        Debug.Log("Iniciou um Novo Jogo");
        byLoad = false;
        SceneManager.LoadScene(newGameLevel);
    }

    public void NewGameCheat()
    {
        SceneManager.LoadScene(newGameLevelCheat);
    }

    public void LoadGame()
    {
        //Debug.Log("Carregou o jogo a partir do Save");
        //savingLoading.loadByMenu = true;
        byLoad = true;
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
