using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CastSpellState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_CastSpellState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Casting Spell");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Applying spell effects");
    }
}