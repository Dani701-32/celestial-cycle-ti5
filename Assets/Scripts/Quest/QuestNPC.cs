using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestNPC : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Player player;
    private InputAction interact;
    private NPCDialogue dialogueSystem;

    [Header("NPC data")]
    public NPCData NPC;

    [SerializeField]
    private int currentQuestAvailable;

    [Header("NPCQuests")]
    [SerializeField]
    private List<QuestStructure> quests;

    [SerializeField]
    public Quest activeQuest;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.gameController;
        dialogueSystem = gameController._NPCDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.playerMovement.interactAction.triggered)
        {
            Interact();
        }
    }

    private void Interact()
    {
        player.playerMovement.enabled = false;
        UpdadeScreen();
    }

    private void UpdadeScreen()
    {
        dialogueSystem.NPCsprite.sprite = NPC.NPCsprite;
        dialogueSystem.textNPCName.text = NPC.Name;
        activeQuest = new Quest(quests[currentQuestAvailable]);
        dialogueSystem.OpenScreen(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
        }
    }
}
