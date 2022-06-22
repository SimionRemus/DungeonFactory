using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_IntroCinematicState : IState
{
    //Write attributes/properties here
    private SCR_ActualSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_IntroCinematicState(SCR_ActualSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Entering Intro Cinematic");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Showing Intro Cinematics here");
    }
}

