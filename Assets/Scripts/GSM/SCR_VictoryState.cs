using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_VictoryState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_VictoryState()
    {

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
