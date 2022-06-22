using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EndPlayerTurnState : IState
{
    //Write attributes/properties here
    private SCR_ActualSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EndPlayerTurnState(SCR_ActualSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Ending player turn");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}