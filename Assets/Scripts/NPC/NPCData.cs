using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC/New NPC", order = 0)]
public class NPCData : ScriptableObject
{
    public string Name;
    public Sprite NPCsprite;
}

public enum Speeker
{
    NPC,
    Player
}

public class Sentence{
    public Speeker speaker; 
    public string message;

}
