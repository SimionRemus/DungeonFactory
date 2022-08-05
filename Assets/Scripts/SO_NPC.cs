using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPCs/NPC")]
public class SO_NPC : ScriptableObject
{
    public int HP;
    public int HPMax;
    public string npcName;
    public NPCtypes npcType;
    public elementType elementType;
    public difficultyType difficulty;
    public List<Vector2Int> movement;
}


public enum NPCtypes
{
    EasyNPC,
    MediumNPC,
    HardNPC
}

public enum difficultyType
{
    Easy,
    Medium,
    Hard
}