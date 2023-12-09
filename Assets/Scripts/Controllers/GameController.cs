using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
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
    public DayNightCycle dayNightController;
    
    [HideInInspector] public bool isMenu = false;
    private string currentCameraX = "";
    private string currentCameraY = "";

    [Header("UI")]
    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject tutorialScreen,
        tutorialDescription;
    public GameObject HUD;

    [SerializeField]
    private GameObject deathScreen,
        popoutGame, menuPrincipalScreen;
    public GameObject tutorialArtefact,
        tutorialCombat, popBackMenu;
    [SerializeField] private Image tabImage; 


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
        deathScreen.SetActive(false); 
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
        freelookCamera.m_XAxis.m_InputAxisValue = 0;
        freelookCamera.m_YAxis.m_InputAxisValue = 0;
        freelookCamera.m_XAxis.m_InputAxisName = currentCameraX;
        freelookCamera.m_YAxis.m_InputAxisName = currentCameraY;
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
                if(!player.hasWeapon && player.playerMovement.currentWeapon != null){
                    player.playerMovement.UniqueppedWeapon(); 
                }
                Time.timeScale = 1.0f;
                isMenu = false;
                tabImage.enabled = true;
                dayNightController.ActiveUI(true);
            }
            else
            {
                GameController.gameController.player.playerMovement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                menuScreen.SetActive(true);
                Time.timeScale = 0f;
                StopCamera();
                tabImage.enabled = false;
                isMenu = true;
                dayNightController.ActiveUI(false);
            }
        }
        
    }

    public void QuitGame()
    {
        popoutGame.SetActive(false);
        popBackMenu.SetActive(false);
        deathScreen.SetActive(false);
        GameController.gameController.isMenu = false;
        GameController.gameController.MenuScreen();
        menuPrincipalScreen.SetActive(true);
        menuScreen.SetActive(false);
        menuController.canvasGame.SetActive(false);
        menuController.uiDayNight.SetActive(false);
        menuController.mainMenuBackground.SetActive(true);
        menuController.CheckSave(GameController.gameController.savingLoadingController.StatusFile());
        GameController.gameController.inventorySystem.ClearInventoryItens();
        GameController.gameController.inventorySystem.ClearInventoryArtifacts();
        GameController.gameController.inventorySystem.StartInventory();
        GameController.gameController.dayNightController.StartDayNightSystem();
        gameController.player.weaponSprite.enabled = false;
        gameController.player.RemoveWeapon(); 
        for (int i = 0; i < 4; i++)
        {
            gameController.player.RemoveArtifact(i); 
            
        }

        SwitchToCameraVirtual(menuCamera);
    }

    public void SwitchToCameraVirtual(CinemachineVirtualCamera targetCamera)
    {
        freelookCamera.GetComponent<CinemachineFreeLook>().Priority = 9;
        menuCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;

        foreach (CinemachineFreeLook camera in freeLookCameras)
        {
            camera.enabled = false;
            targetCamera.enabled = true;
        }
    }

    public void SwitchToCameraFreeLook(CinemachineFreeLook targetCamera)
    {
        freelookCamera.GetComponent<CinemachineFreeLook>().Priority = 10;
        menuCamera.GetComponent<CinemachineVirtualCamera>().Priority = 9;

        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            camera.enabled = false;
            targetCamera.enabled = true;
        }
    }
}
