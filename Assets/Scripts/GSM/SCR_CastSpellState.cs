using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SCR_CastSpellState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;

    private Tilemap groundTilemap;
    private GameObject selectedCard;
    private Vector2Int[] spellRange;
    private Vector2Int[] spellRangePositions; //used to remove spell range when nothing is selected.
    private bool tileEffectApplied;
    
    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_CastSpellState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = true;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        groundTilemap = stateMachine.grid.transform.Find("GroundTilemap").GetComponent<Tilemap>();
        spellRangePositions = new Vector2Int[25];
        tileEffectApplied = false;
        Debug.Log("entered cast spell");
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        

        Debug.Log("exiting cast spell");
    }

    void IState.OnUpdate()
    {
        var cardSelectionBox = stateMachine.GameUI.transform.Find("CardSelectionBox").GetComponent<Image>();
        if (cardSelectionBox.enabled)
        {
            GameObject actionlist = stateMachine.GameUI.transform.Find("ActionList").gameObject;
            int cardnumber = stateMachine.GameUI.transform.Find("ActionList").childCount;
            for (int i = 0; i < cardnumber; i++)
            {
                var currentCard = actionlist.transform.GetChild(i);
                if (currentCard.position == cardSelectionBox.rectTransform.position)
                {
                    selectedCard = currentCard.gameObject;

                    Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
                    Vector2Int playerTilePos = new Vector2Int((int)playerPos.x, (int)playerPos.y);
                    spellRange = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.SpellRange;
                    for (int j = 0; j < spellRange.Length; j++)
                    {
                        Vector2Int sRTPos = playerTilePos + new Vector2Int(spellRange[j].x, spellRange[j].y);
                        spellRangePositions[j] = sRTPos;
                        groundTilemap.SetTileFlags(new Vector3Int(sRTPos.x, sRTPos.y, 0), TileFlags.None);
                        groundTilemap.SetColor(new Vector3Int(sRTPos.x, sRTPos.y, 0), new Color32(100, 100, 255, 255));
                        groundTilemap.SetTileFlags(new Vector3Int(sRTPos.x, sRTPos.y, 0), TileFlags.LockAll);
                    }
                }
            }
            if (!tileEffectApplied)
            {
                tileEffectApplied = true;
                Vector3 pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Vector3Int tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);
                TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(tilePos);
                if (tileground != null)
                {
                    GameObject target = stateMachine.transform.Find("tileEffectSprite").gameObject;
                    GameObject tileEffect = GameObject.Instantiate(target, pos + new Vector3(0.5f, 0.5f), Quaternion.identity);
                    tileEffect.SetActive(true);

                    //TO BE completed (all 7x7 possibilities)
                    SpellEnvironmentalEffect(selectedCard, tilePos);

                    //Check if anything is on tile:
                    Vector3 midpoint = groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);
                    Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f);
                    if (collider)
                    {
                        AffectUnitOnTile(collider.gameObject);
                    }
                    //EXIT the state
                    stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
                }
            }
            else
            {
                for (int k = 0; k < spellRangePositions.Length; k++)
                {
                    groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.None);
                    groundTilemap.SetColor(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), new Color32(255, 255, 255, 255));
                    groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.LockAll);
                    spellRangePositions[k] = new Vector2Int(0, 0);
                }
            }
        }
        else
        {
            for (int k = 0; k < spellRangePositions.Length; k++)
            {
                groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.None);
                groundTilemap.SetColor(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), new Color32(255, 255, 255, 255));
                groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.LockAll);
                spellRangePositions[k] = new Vector2Int(0, 0);
            }
        }
    }

    private void SpellEnvironmentalEffect(GameObject selectedCard, Vector3Int tilePos)
    {
        TileWithAttributes tile = (TileWithAttributes)groundTilemap.GetTile(tilePos);

        switch (selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.mandatoryElement)
        {
            case elementType.None:
                break;
            case elementType.Earth:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        break;
                    case elementType.Water:
                        //Water healing and damage spells are halved on this tile this turn.
                        tile.tileEffect = tileEffect.waterHalfEffect;
                        break;
                    case elementType.Fire:
                        //Fire healing and damage spells are halved on this tile this turn.
                        break;
                    case elementType.Air:
                        //Causes double damage if spell is earth attack this turn.
                        break;
                    case elementType.Divination:
                        //turn to random tile between [dirt(55%)], [volcanic(15%)], [swamp(15%)] or [sand(15%)]
                        float v = Random.Range(0f, 1f);
                        switch (v)
                        {
                            case float n when n < 0.55f:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[0]);
                                break;
                            case float n when n < 0.7f:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[1]);
                                break;
                            case float n when n < 0.85f:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[2]);
                                break;
                            default:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[3]);
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of earth type will cause either double or no effect.
                        break;
                    case elementType.Life:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[0]);
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Water:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[1]);
                        break;
                    case elementType.Water:
                        break;
                    case elementType.Fire:
                        //Causes 2x damage if spell is water attack this turn.
                        break;
                    case elementType.Air:
                        //Tile is considered 2 tiles for movement this turn.
                        break;
                    case elementType.Divination:
                        int v = Random.Range(0, 3);
                        //Random effect: [turn to swamp], [nothing], [causes x1.5 damage on this tile this turn]
                        switch (v)
                        {
                            case 0:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[1]);
                                break;
                            case 1:
                                //[nothing]
                                break;
                            case 2:
                                //[causes x1.5 damage on this tile this turn]
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of earth type will cause either double or no effect.
                        break;
                    case elementType.Life:
                        //Life spells are played twice on this tile this turn
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Fire:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[2]);
                        break;
                    case elementType.Water:
                        //Causes x1.25 damage on this tile this turn.
                        break;
                    case elementType.Fire:
                        break;
                    case elementType.Air:
                        //Causes 2x damage if spell is fire attack this turn.
                        break;
                    case elementType.Divination:
                        int v = Random.Range(0, 3);
                        //Random effect: [turn to volcanic], [nothing], [causes x2 damage on this tile if spell is fire-attack]
                        switch (v)
                        {
                            case 0:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[2]);
                                break;
                            case 1:
                                //[nothing]
                                break;
                            case 2:
                                //[causes x2 damage on this tile if spell is fire-attack]
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of earth type will cause either double or no effect.
                        break;
                    case elementType.Life:
                        //Life spells have no effect on this tile this turn (still consumes willpower).
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Air:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[3]);
                        break;
                    case elementType.Water:
                        //Tile halves healing on it for this turn.
                        break;
                    case elementType.Fire:
                        //Causes 2x damage if spell is air-attack this turn.
                        break;
                    case elementType.Air:
                        break;
                    case elementType.Divination:
                        int v = Random.Range(0, 3);
                        //Random effect: [turn to sand], [nothing], [Tile is considered 2 tiles for movement this turn]
                        switch (v)
                        {
                            case 0:
                                groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[3]);
                                break;
                            case 1:
                                //[nothing]
                                break;
                            case 2:
                                //[Tile is considered 2 tiles for movement this turn]
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of earth type will cause either double or no effect.
                        break;
                    case elementType.Life:
                        //Neighboring tiles receive 5 damage.
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Divination:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[4]);
                        break;
                    case elementType.Water:
                        //Heals for 5HP if passed through this turn.
                        break;
                    case elementType.Fire:
                        //Damages for 5HP if passed through this turn.
                        break;
                    case elementType.Air:
                        //Free movement this turn.
                        break;
                    case elementType.Divination:
                        break;
                    case elementType.Illusion:
                        //Illusion spells cast here receive no bonus from optional components this turn.
                        break;
                    case elementType.Life:
                        if (Random.Range(0, 2) == 0)
                            groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[4]);
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Illusion:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[5]);
                        break;
                    case elementType.Water:
                        //Healing spells cause damage instead this turn
                        break;
                    case elementType.Fire:
                        //Damage spells cause healing instead this turn
                        break;
                    case elementType.Air:
                        //50% chance to spawn NPC (event or monster)
                        break;
                    case elementType.Divination:
                        if (Random.Range(0, 2) == 0)
                            groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[5]);
                        break;
                    case elementType.Illusion:
                        break;
                    case elementType.Life:
                        //Heals 10HP if turn ends here.
                        break;
                    default:
                        break;
                }
                break;
            case elementType.Life:
                switch (tile.element)
                {
                    case elementType.None:
                        break;
                    case elementType.Earth:
                        groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[6]);
                        break;
                    case elementType.Water:
                        //Will consume 1 torch when passed through this turn.
                        break;
                    case elementType.Fire:
                        //Lights up room. won't use torch at the end of this turn if in the same room.
                        break;
                    case elementType.Air:
                        //Creates wall for this turn if no one there. 10DMG to creature there otherwise
                        break;
                    case elementType.Divination:
                        //Causes 10DMG if turn ends here.
                        break;
                    case elementType.Illusion:
                        if (Random.Range(0, 2) == 0)
                            groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[6]);
                        break;
                    case elementType.Life:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void AffectUnitOnTile(GameObject gObject)
    {
        Debug.Log(gObject.name);
    }

    
}