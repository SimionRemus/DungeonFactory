using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SCR_PlayerTurnIdleState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private Tilemap groundTilemap;
    private Tilemap wallTilemap;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}]", pos.x, pos.y));
            TileWithAttributes tileground = (TileWithAttributes)groundTilemap.GetTile(new Vector3Int((int)pos.x, (int)pos.y, 0));
            TileWithAttributes tilewall = (TileWithAttributes)wallTilemap.GetTile(new Vector3Int((int)pos.x, (int)pos.y, 0));
            if (tileground != null)
            {
                //Detect what kind of tile it is.
                Debug.Log(string.Format("Tile is: {0}", tileground.tileName));
                //Check if anything is on tile:
                Vector3 midpoint=groundTilemap.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0))+new Vector3(0.5f,0.5f,0);
                Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f);
                if (collider)
                {
                        Debug.Log(collider.gameObject.name);
                }
                //Change tile with another tile (example changes to Life.
                //groundTilemap.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), stateMachine.grid.GetComponent<SCR_FloorGeneration>().tilePrefabs[6]);
            }
        }
    }
}
