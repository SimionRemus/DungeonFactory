using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_VictoryState : IState
{
    //Write attributes/properties here
    private SCR_ActualSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_VictoryState(SCR_ActualSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Game Won");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Do stuff on game won");
    }
}
