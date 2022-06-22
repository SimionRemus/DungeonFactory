using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MainMenuStates : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_MainMenuStates()
    {
        
    }

    void IState.OnEnter()
    {
        Debug.Log("Game started");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Showing Main menu here");
    }
}
