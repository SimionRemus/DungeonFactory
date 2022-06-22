using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SettingsState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SettingsState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Settings clicked");
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
