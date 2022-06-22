using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SaveDataState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SaveDataState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Start saving data");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Do data saving.");
    }
}
