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


    private Vector3Int tilePos;
    private Vector3 pos;
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
                }
            }
            if (!tileEffectApplied)
            {
                tileEffectApplied = true;
                pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);
                TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(tilePos);
                if (tileground != null)
                {
                    //Check if anything is on tile:
                    
                }
            }
            if (HasTheComponents())
            {
                
                Vector3 midpoint = groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);
                Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f);
                //if (collider)
                //{
                //    AffectUnitOnTile(collider.gameObject);
                //}
                SpellEnvironmentalEffect(selectedCard, tilePos);
                bool didSpellWork = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.DoSpellEffects(stateMachine.player, groundTilemap, stateMachine, tilePos, collider);
                var movepoint = GameObject.Find("Movepoint").transform.position;
                var playerPos = stateMachine.player.transform.position;
                playerPos = Vector3.MoveTowards(playerPos, movepoint, stateMachine.player.GetComponent<SCR_Player>().moveSpeed * Time.deltaTime);
                if (Vector3.Distance(playerPos, movepoint) <= 0.05f)
                {
                    if(didSpellWork)
                    {
                        stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower -= selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardCost;
                    }
                    //EXIT the state
                    stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
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
                        //Causes half damage or heal if spell is water this turn.
                        tile.tileEffect = tileEffect.waterHalfEffect;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        //Causes half damage or heal if spell is fire this turn.
                        tile.tileEffect = tileEffect.fireHalfEffect;
                        SetTileEffectMarker();
                        break;
                    case elementType.Air:
                        //Causes double damage if spell is earth attack this turn.
                        tile.tileEffect = tileEffect.doubleEarthDamage;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.doubleOrNothingEarthDamage;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.doubleWaterDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Air:
                        //Tile is considered 2 tiles for movement this turn.
                        tile.tileEffect = tileEffect.halfSpeed;
                        SetTileEffectMarker();
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
                                tile.tileEffect = tileEffect.extraFiftyPercentDamage;
                                SetTileEffectMarker();
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of water type will cause either double or no effect.
                        tile.tileEffect = tileEffect.doubleOrNothingWaterDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Life:
                        //Life spells are played twice on this tile this turn
                        tile.tileEffect = tileEffect.lifeTwice;
                        SetTileEffectMarker();
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
                        //Causes x1.5 damage on this tile this turn.
                        tile.tileEffect = tileEffect.extraFiftyPercentDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        break;
                    case elementType.Air:
                        //Causes 2x damage if spell is fire attack this turn.
                        tile.tileEffect = tileEffect.doubleFireDamage;
                        SetTileEffectMarker();
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
                                tile.tileEffect = tileEffect.doubleFireDamage;
                                SetTileEffectMarker();
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of fire type will cause either double or no effect.
                        tile.tileEffect = tileEffect.doubleOrNothingFireDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Life:
                        //Life spells have no effect on this tile this turn (still consumes willpower).
                        tile.tileEffect = tileEffect.lifeCancel;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.halfHealEffect;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        //Causes 2x damage if spell is air-attack this turn.
                        tile.tileEffect = tileEffect.doubleAirDamage;
                        SetTileEffectMarker();
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
                                tile.tileEffect = tileEffect.halfSpeed;
                                SetTileEffectMarker();
                                break;
                            default:
                                break;
                        }
                        break;
                    case elementType.Illusion:
                        //Spells of air type will cause either double or no effect.
                        tile.tileEffect = tileEffect.doubleOrNothingAirDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Life:
                        //Neighboring tiles receive 5 damage.
                        tile.tileEffect = tileEffect.dmgNeighboursFive;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.healFiveIfPassed;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        //Damages for 5HP if passed through this turn.
                        tile.tileEffect = tileEffect.dmgFiveIfPassed;
                        SetTileEffectMarker();
                        break;
                    case elementType.Air:
                        //Free movement this turn.
                        tile.tileEffect = tileEffect.doubleSpeed;
                        SetTileEffectMarker();
                        break;
                    case elementType.Divination:
                        break;
                    case elementType.Illusion:
                        //Illusion spells cast here receive no bonus from optional components this turn.
                        tile.tileEffect = tileEffect.noBonusOnIllusion;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.healToDamage;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        //Damage spells cause healing instead this turn
                        tile.tileEffect = tileEffect.dmgToHeal;
                        SetTileEffectMarker();
                        break;
                    case elementType.Air:
                        //50% chance to spawn NPC (event or monster)
                        //TO DO
                        break;
                    case elementType.Divination:
                        if (Random.Range(0, 2) == 0)
                            groundTilemap.SetTile(tilePos, stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[5]);
                        break;
                    case elementType.Illusion:
                        break;
                    case elementType.Life:
                        //Heals 10HP if turn ends here.
                        tile.tileEffect = tileEffect.healTenOnEOT;
                        SetTileEffectMarker();
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
                        tile.tileEffect = tileEffect.useTorchIfPassed;
                        SetTileEffectMarker();
                        break;
                    case elementType.Fire:
                        //Lights up room. won't use torch at the end of this turn if in the same room.
                        tile.tileEffect = tileEffect.noTorchUsed;
                        SetTileEffectMarker();
                        break;
                    case elementType.Air:
                        //Creates wall for this turn if no one there. 10DMG to creature there otherwise
                        tile.tileEffect = tileEffect.createWall;
                        SetTileEffectMarker();
                        break;
                    case elementType.Divination:
                        //Causes 10DMG if turn ends here.
                        tile.tileEffect = tileEffect.dmgTenOnEOT;
                        SetTileEffectMarker();
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

    private bool HasTheComponents()
    {
        elementType[] infusionSlots = stateMachine.player.GetComponent<SCR_Player>().infusionslots;
        SO_Spell spellCard = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard;
        int mandatoryNumberOfSlots = 0;
        for (int i = 0; i < infusionSlots.Length; i++)
        {
            if (spellCard.mandatoryElement == infusionSlots[i])
            {
                mandatoryNumberOfSlots++;
            }
        }
        if (mandatoryNumberOfSlots >= spellCard.mandatoryElementAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetTileEffectMarker()
    {
        GameObject target = stateMachine.transform.Find("tileEffectSprite").gameObject;
        GameObject tileEffectGO = GameObject.Instantiate(target, pos + new Vector3(0.5f, 0.5f), Quaternion.identity);
        tileEffectGO.SetActive(true);
        tileEffectGO.transform.SetParent(GameObject.Find("TileEffectContainer").transform);
    }
}