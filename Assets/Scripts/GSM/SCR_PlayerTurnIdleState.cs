using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerTurnIdleState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_PlayerTurnIdleState()
    {
        
    }

    void IState.OnEnter()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Showing Settings here");
    }
}
