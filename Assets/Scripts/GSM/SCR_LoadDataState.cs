using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LoadDataState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_LoadDataState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Continue game clicked");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Loading game data here");
    }
}