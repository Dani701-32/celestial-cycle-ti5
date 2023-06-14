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
    public Transform cam;
    public CinemachineFreeLook freelookCamera;
    private bool isMenu = false;
    private string currentCameraX = "";
    private string currentCameraY = "";

    [Header("UI")]
    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject deathScreen, popoutGame;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = (gameController == null) ? this : gameController;
        inventorySystem = GetComponent<InventorySystem>();
        questSystem = GetComponent<QuestSystem>();
        _NPCDialogue = GetComponent<NPCDialogue>();
        currentCameraX = "Mouse X";
        currentCameraY = "Mouse Y";
    }

    void Start()
    {
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

    public void MenuScreen()
    {
        if (isMenu)
        {
            GameController.gameController.player.playerMovement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            inventorySystem.CloseScreen();
            questSystem.CloseScreen();
            menuScreen.SetActive(false);
            popoutGame.SetActive(false);
            ReleaseCamera();
            isMenu = false;
        }
        else
        {
            GameController.gameController.player.playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            menuScreen.SetActive(true);

            StopCamera();
            isMenu = true;
        }
    }

    public void QuitGame(){
        popoutGame.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
