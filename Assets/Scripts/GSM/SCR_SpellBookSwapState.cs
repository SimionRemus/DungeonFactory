using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SpellBookSwapState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SpellBookSwapState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Opening spellbook");
    }

    void IState.OnExit()
    {
        Debug.Log("Closing spellbook");
    }

    void IState.OnUpdate()
    {
        throw new System.NotImplementedException();
    }

}
