using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DoEventTileState : IState
{
    //Write attributes/properties here
    private SCR_ActualSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_DoEventTileState(SCR_ActualSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Event reached");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Apply event");
    }
}
