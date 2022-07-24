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

    public bool DoSpellEffects(GameObject player, Tilemap groundTilemap,SCR_GameSM stateMachine,Vector3Int clickedTilePos,Collider2D collider)
    {
        Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
        Vector2Int playerPosition= new Vector2Int((int)playerPos.x, (int)playerPos.y);
        Transform movepoint;
        int v;
        switch (SpellID)
        {
            case SpellList.None:
                return false;
            case SpellList.MoveNEWS2:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveNEWS3:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveDiagonal2:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveDiagonal3:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveAnywhere1:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveAnywhere2:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.MoveAnywhere3:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                break;
            case SpellList.MoveChessHorse:
                movepoint = GameObject.Find("Movepoint").transform;
                movepoint.position = clickedTilePos + new Vector3(0.5f, 0.5f);
                return true;
            case SpellList.HitWithTorch:
                if (collider != null)
                {
                    if (collider.gameObject.TryGetComponent<SCR_NPC>(out var npc))
                    {
                        v = Random.Range(0, 3);
                        if (v == 0)
                        {
                            player.GetComponent<SCR_Player>().numberOfTorches--;
                        }
                        npc.AffectHitpoints(10);
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            case SpellList.Meditate:
                player.GetComponent<SCR_Player>().numberOfWillpowerModifier += 1;
                return true;
            case SpellList.Premonition:
                break;
            case SpellList.EternalTorch:
                player.GetComponent<SCR_Player>().numberOfTorchesModifier += 1;
                return true; ;
            default:
                return false;
        }
        return false;
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
    MoveNEWS3,
    MoveDiagonal2,
    MoveDiagonal3,
    MoveAnywhere1,
    MoveAnywhere2,
    MoveAnywhere3,
    MoveChessHorse,
    HitWithTorch,
    Meditate,
    Premonition,
    EternalTorch,


}