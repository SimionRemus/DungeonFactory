using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EndPlayerTurnState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EndPlayerTurnState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Ending player turn");
    }

    void IState.OnExit()
    {
        Debug.Log("Ending enemy turn for debug purposes");
    }

    void IState.OnUpdate()
    {
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }

}