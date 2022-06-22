using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DoEventTileState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_DoEventTileState()
    {

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
