using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_UpgradeSpellState : IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_UpgradeSpellState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Entering UpdateSpell");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Doing upgrade spell stuff here");
    }
}
