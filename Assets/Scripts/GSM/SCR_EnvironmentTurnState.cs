using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnvironmentTurnState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EnvironmentTurnState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Environment turn started");
        stateMachine.player.GetComponent<SCR_Player>().UpdateWillpower();
        stateMachine.player.GetComponent<SCR_Player>().UpdateTorches();
        stateMachine.player.GetComponent<SCR_Player>().UpdateHitpoints();
        stateMachine.player.GetComponent<SCR_Player>().numberOfWillpowerModifier = 0;
        stateMachine.player.GetComponent<SCR_Player>().numberOfHitpointsModifier = 0;
        stateMachine.player.GetComponent<SCR_Player>().numberOfTorchesModifier = 0;

        stateMachine.grid.transform.GetComponent<SCR_FloorGeneration>().ToggleDoors();
    }

    void IState.OnExit()
    {
        Debug.Log("Environment turn ended");
    }
    

    void IState.OnUpdate()
    {
        Debug.Log("Doing environ stuff here");
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
}
