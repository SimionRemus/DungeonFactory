using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SCR_DoEventTileState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_DoEventTileState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Event reached");
    }

    void IState.OnExit()
    {
        var groundTilemap = stateMachine.grid.transform.Find("GroundTilemap").GetComponent<Tilemap>();
        Vector3 midpoint = groundTilemap.WorldToCell(stateMachine.player.transform.position) + new Vector3(0.5f, 0.5f, 0);
        Collider2D collider = Physics2D.OverlapCircle(midpoint, 0.45f, 2048);
        if (collider)
        {
            GameObject.Destroy(collider.gameObject);
        }
        Debug.Log("Event Finished");
    }

    void IState.OnUpdate()
    {
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
}
