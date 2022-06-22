using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnvironmentTurnState : IState
{
    //Write attributes/properties here
    private SCR_ActualSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EnvironmentTurnState(SCR_ActualSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Environment turn started");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }
    

    void IState.OnUpdate()
    {
        Debug.Log("Doing environ stuff here");
    }
}
