﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DefeatState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_DefeatState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Game Lost");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Doing stuff on game lost");
    }
}
