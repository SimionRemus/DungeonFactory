using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class SCR_PlayerTurnIdleState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private Tilemap groundTilemap;
    private GameObject selectedCard;
    private Vector2Int[] spellRange;
    private Vector2Int[] spellRangePositions; //used to remove spell range when nothing is selected.

    public Button endTurn;
    public Button spellbook;
    public int newTurnWillpower;

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
        spellRangePositions = new Vector2Int[25];


        endTurn = stateMachine.GameUI.transform.Find("EndTurn").GetComponent<Button>();
        endTurn.onClick.AddListener(EndTurnClicked);

        spellbook = stateMachine.GameUI.transform.Find("OpenSpellbook").GetComponent<Button>();
        spellbook.onClick.AddListener(SpellbookClicked);

        newTurnWillpower = 3;

        Debug.Log("playerTurnIdleState");

    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;


        endTurn.onClick.RemoveListener(EndTurnClicked);
        spellbook.onClick.RemoveListener(SpellbookClicked);
    }

    void IState.OnUpdate()
    {
        #region CheckForWinCondition
        if (stateMachine.WINCONDITION)
        {
            stateMachine.ChangeState(stateMachine.VictoryState);
        }
        #endregion
        #region CheckForEventWherePlayerIs
        Vector3 midpoint = groundTilemap.WorldToCell(stateMachine.player.transform.position) + new Vector3(0.5f, 0.5f, 0);
        Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f,2048);
        if (collider)
        {
            stateMachine.ChangeState(stateMachine.DoEventTileState);
        }
        #endregion
        #region CheckForSpellFired
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

                    Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
                    Vector2Int playerTilePos = new Vector2Int((int)playerPos.x, (int)playerPos.y);
                    spellRange = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.SpellRange;
                    for (int j = 0; j < spellRange.Length; j++)
                    {
                        Vector2Int sRTPos = playerTilePos + new Vector2Int(spellRange[j].x, spellRange[j].y);
                        spellRangePositions[j] = sRTPos;
                        HighlightRange(true, j);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0)&& (!EventSystem.current.IsPointerOverGameObject())) //Clicked inside gamespace (i.e. a tile)
            {
                Vector3 pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                
                Vector3Int tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);

                TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(tilePos);
                if (tileground != null)
                {
                    if (ValidateSpell(tilePos))
                    {
                        for (int k = 0; k < spellRangePositions.Length; k++)
                        {
                            HighlightRange(false, k);
                        }
                        stateMachine.ChangeState(stateMachine.CastSpellState);
                    }
                }
            }
        }
        else
        {
            for (int k = 0; k < spellRangePositions.Length; k++)
            {
                HighlightRange(false, k);
            }
        }
        #endregion
    }



    private void EndTurnClicked()
    {
        //Reset willpower and remove one torch
        stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower = newTurnWillpower;
        stateMachine.player.GetComponent<SCR_Player>().numberOfTorches -= 1;
        //Remove Tile Effect markers
        GameObject TEC = GameObject.Find("TileEffectContainer");
        for (int i = 0; i < TEC.transform.childCount; i++)
        {
            GameObject.Destroy(TEC.transform.GetChild(i).gameObject);
        }
        //Change state
        stateMachine.ChangeState(stateMachine.EndPlayerTurnState);
    }

    private void SpellbookClicked()
    {
        stateMachine.ChangeState(stateMachine.SpellbookSwapState);
    }

    private bool ValidateSpell(Vector3Int tilePos)
    {

        //Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
        //Vector2Int playerTilePos = new Vector2Int((int)playerPos.x, (int)playerPos.y);
        Vector2Int tilepos2D = new Vector2Int(tilePos.x, tilePos.y);
        SO_Spell spell = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard;
        if (spell.cardCost > stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower)
        {
            return false;
        }
        for (int k = 0; k < spellRangePositions.Length; k++)
        {
            if (spellRangePositions[k] == tilepos2D)
            {
                return true;
            }
        }

        return false;

    }

    private void HighlightRange(bool highlight,int k)
    {
        if(highlight)
        {
            groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.None);
            groundTilemap.SetColor(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), new Color32(100, 100, 255, 255));
            groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.LockAll);
        }
        else
        {
            groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.None);
            groundTilemap.SetColor(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), new Color32(255, 255, 255, 255));
            groundTilemap.SetTileFlags(new Vector3Int(spellRangePositions[k].x, spellRangePositions[k].y, 0), TileFlags.LockAll);
            spellRangePositions[k] = new Vector2Int(0, 0);
        }
    }
}
