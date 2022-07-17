using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card")]
public class SO_Spell : ScriptableObject
{
    public SpellList SpellID;
    public Sprite cardImage;
    public int cardCost;
    public string cardDescription;
    public string cardName;
    public elementType mandatoryElement;
    public int mandatoryElementAmount;
    [SerializeField] elementType[] optionalElements;
    [SerializeField] elementType[] forbiddenElements;
    public Vector2Int[] SpellRange;

    public void DoSpellEffects(GameObject player, Tilemap groundTilemap,SCR_GameSM stateMachine,Vector3Int clickedTilePos)
    {
        Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
        Vector2Int playerPosition= new Vector2Int((int)playerPos.x, (int)playerPos.y);
        switch (SpellID)
        {
            case SpellList.None:
                break;
            case SpellList.MoveNEWS2:
                Transform movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos+new Vector3(0.5f,0.5f);
                break;
            case SpellList.BasicFireSpell:
                break;
        }

    }
}

public enum elementType
{
    None,
    Earth,
    Water,
    Fire,
    Air,
    Divination,
    Illusion,
    Life
}

public enum SpellList
{
    None,
    MoveNEWS2,
    BasicFireSpell
}