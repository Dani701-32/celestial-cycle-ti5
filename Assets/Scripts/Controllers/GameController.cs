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
    public NPCDialogue _NPCDialogue;
    public Player player;
    public CinemachineFreeLook freelookCamera;
    private bool isMenu = false;
    private string currentCameraX = "";
    private string currentCameraY = "";

    [Header("UI")]
    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject deathScreen;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = (gameController == null) ? this : gameController;
        inventorySystem = GetComponent<InventorySystem>();
        _NPCDialogue = GetComponent<NPCDialogue>();
        currentCameraX = "Mouse X";
        currentCameraY = "Mouse Y";
    }

    void Start()
    {
        menuScreen.SetActive(false);
        deathScreen.SetActive(false);
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

    public void MenuScreen()
    {
        if (isMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            menuScreen.SetActive(false);
            currentCameraX = "Mouse X";
            currentCameraY = "Mouse Y";
            isMenu = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            menuScreen.SetActive(true);
            currentCameraX = "";
            currentCameraY = "";
            isMenu = true;
        }
    }
}
