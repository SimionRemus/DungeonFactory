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
    private tileOffset[] spellRange;
    private Vector3Int[] spellRangePositions; //used to remove spell range when nothing is selected.

    public Button endTurn;

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
        spellRangePositions = new Vector3Int[25];


        endTurn = stateMachine.GameUI.transform.Find("EndTurn").GetComponent<Button>();
        endTurn.onClick.AddListener(EndTurnClicked);
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

                    Vector3 playerPos = groundTilemap.WorldToCell(stateMachine.player.transform.position);
                    Vector3Int playerTilePos = new Vector3Int((int)playerPos.x, (int)playerPos.y, 0);
                    TileWithAttributes playerTile = (TileWithAttributes)groundTilemap.GetTile(playerTilePos);
                    spellRange = selectedCard.GetComponent<SCR_CardInfoDisplay>().SpellCard.SpellRange;
                    for (int j = 0; j < spellRange.Length; j++)
                    {
                        Vector3Int sRTPos = playerTilePos + new Vector3Int(spellRange[j].x, spellRange[j].y, 0);
                        spellRangePositions[j] = sRTPos;
                        groundTilemap.SetTileFlags(sRTPos, TileFlags.None);
                        groundTilemap.SetColor(sRTPos, new Color32(100, 100, 255, 255));
                        groundTilemap.SetTileFlags(sRTPos, TileFlags.LockAll);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0)&& (!EventSystem.current.IsPointerOverGameObject()))
            {
                Vector3 pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                
                Vector3Int tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);

                TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(tilePos);
                if (tileground != null)
                {
                    stateMachine.ChangeState(stateMachine.CastSpellState);
                }
            }
        }
        else
        {
            for (int k = 0; k < spellRangePositions.Length; k++)
            {
                groundTilemap.SetTileFlags(spellRangePositions[k], TileFlags.None);
                groundTilemap.SetColor(spellRangePositions[k], new Color32(255, 255, 255, 255));
                groundTilemap.SetTileFlags(spellRangePositions[k], TileFlags.LockAll);
                spellRangePositions[k] = new Vector3Int(0, 0, 0);
            }
        }
    }
    


    private void EndTurnClicked()
    {
        stateMachine.ChangeState(stateMachine.EndPlayerTurnState);
    }
}
