using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    [Header("UI")]
    [SerializeField]
    private GameObject deatScreen;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = (gameController == null) ? this : gameController;
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }
}
