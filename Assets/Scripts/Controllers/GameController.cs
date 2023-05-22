using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    [Header("UI")]
    [SerializeField]
    private GameObject deathScreen;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = (gameController == null) ? this : gameController;
    }

    void Start()
    {
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() { }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void ResetScene()
    {
        int currectScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currectScene);
    }
}
