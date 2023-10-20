using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public InventorySystem inventorySystem;
    public QuestSystem questSystem;
    public NPCDialogue _NPCDialogue;
    public Player player;
    public SaveObject saveObject;
    public MenuController menuController;
    public SavingLoading savingLoadingController;
    
    [HideInInspector] public bool isMenu = false;
    private string currentCameraX = "";
    private string currentCameraY = "";

    [Header("UI")]
    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject tutorialScreen,
        tutorialDescription;

    [SerializeField]
    private GameObject deathScreen,
        popoutGame;
    public GameObject tutorialArtefact,
        tutorialCombat;


    [Header("Camera Controller")]
    public string triggerTag;
    public Transform cam;
    public CinemachineFreeLook freelookCamera;
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera[] virtualCameras;
    public CinemachineFreeLook[] freeLookCameras;

    // Start is called before the first frame update
    private void Awake()
    {
        gameController = (gameController == null) ? this : gameController;
        inventorySystem = GetComponent<InventorySystem>();
        savingLoadingController = GetComponent<SavingLoading>();
        questSystem = GetComponent<QuestSystem>();
        _NPCDialogue = GetComponent<NPCDialogue>();
        currentCameraX = "Mouse X";
        currentCameraY = "Mouse Y";
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        tutorialScreen.SetActive(false);
        tutorialDescription.SetActive(false);
        menuScreen.SetActive(false);
        deathScreen.SetActive(false);
        popoutGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        freelookCamera.m_XAxis.m_InputAxisName = currentCameraX;
        freelookCamera.m_YAxis.m_InputAxisName = currentCameraY;
    }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void ResetScene()
    {
        int currectScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currectScene);
    }

    public void ReleaseCamera()
    {
        currentCameraX = "Mouse X";
        currentCameraY = "Mouse Y";
    }

    public void StopCamera()
    {
        currentCameraX = "";
        currentCameraY = "";
    }

    public void ContinueGame()
    {
        ReleaseCamera();
        Time.timeScale = 1.0f;
        GameController.gameController.player.playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MenuScreen()
    {
        if(!saveObject.isSave)
        {
            if (isMenu)
            {
                GameController.gameController.player.playerMovement.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                inventorySystem.CloseScreen();
                inventorySystem.CloseArtifactScreen();
                questSystem.CloseScreen();
                menuScreen.SetActive(false);
                popoutGame.SetActive(false);

                tutorialScreen.SetActive(false);
                tutorialDescription.SetActive(false);
                ReleaseCamera();
                Time.timeScale = 1.0f;
                isMenu = false;
            }
            else
            {
                GameController.gameController.player.playerMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                menuScreen.SetActive(true);
                Time.timeScale = 0f;
                StopCamera();
                isMenu = true;
            }
        }
        
    }

    public void QuitGame()
    {
        popoutGame.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void ExitMenuPrincipal()
    {
        //SceneManager.LoadScene(menuController.levelToLoad);
    }

    public void SwitchToCameraVirtual(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineFreeLook camera in freeLookCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }

    public void SwitchToCameraFreeLook(CinemachineFreeLook targetCamera)
    {
        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }

    //public void StartMenuScreen()
    //{
    //    GameController.gameController.player.playerMovement.enabled = false;
    //    Cursor.lockState = CursorLockMode.None;
    //    menuScreen.SetActive(true);
    //    Time.timeScale = 0f;
    //    StopCamera();
    //    isMenu = true;
    //}
}
