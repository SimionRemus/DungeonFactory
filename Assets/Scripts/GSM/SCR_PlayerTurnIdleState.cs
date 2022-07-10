using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class SCR_PlayerTurnIdleState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private Tilemap groundTilemap;
    private Tilemap wallTilemap;
    private GameObject selectedCard;

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_PlayerTurnIdleState(SCR_GameSM SM)
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
        wallTilemap = stateMachine.grid.transform.Find("WallsTilemap").GetComponent<Tilemap>();
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;
    }

    void IState.OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            Debug.Log("Pressed escape. returning to main menu. This is for debug purposes only!!!");
            stateMachine.ChangeState(stateMachine.MainMenuState);
            return;
        }
        var cardSelectionBox = stateMachine.GameUI.transform.Find("CardSelectionBox").GetComponent<Image>();
        if (cardSelectionBox.enabled) 
        {
            GameObject actionlist = stateMachine.GameUI.transform.Find("ActionList").gameObject;
            int cardnumber = stateMachine.GameUI.transform.Find("ActionList").childCount;
            for (int i = 0; i < cardnumber; i++)
            {
                var currentCard=actionlist.transform.GetChild(i);
                if (currentCard.position==cardSelectionBox.rectTransform.position)
                {
                    selectedCard = currentCard.gameObject;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}]", pos.x, pos.y));
                Vector3Int tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);
                TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(tilePos);
                if (tileground != null)
                {
                    //Detect what kind of tile it is.
                    Debug.Log(string.Format("Casting spell {0} on tile {1}.",selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.name ,tileground.element));
                    //TO BE completed (all 7x7 possibilities)
                    SpellEnvironmentalEffect(selectedCard, tilePos);
                    //Check if anything is on tile:
                    Vector3 midpoint = groundTilemap.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0)) + new Vector3(0.5f, 0.5f, 0);
                    Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f);
                    if (collider)
                    {
                        Debug.Log(collider.gameObject.name);
                    }
                }
            }
        }
        else
        {
            //Change tile with another tile (example changes to Life.
            //groundTilemap.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[6]);
            //Set clicked tile to blue tint
            //groundTilemap.SetTileFlags(tilePos, TileFlags.None);
            //groundTilemap.SetColor(tilePos, new Color32(100, 100, 255, 255));
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

}
