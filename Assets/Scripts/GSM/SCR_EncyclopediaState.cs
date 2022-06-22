using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EncyclopediaState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EncyclopediaState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Encyclopedia clicked");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Showing Encyclopedia here");
    }
}
