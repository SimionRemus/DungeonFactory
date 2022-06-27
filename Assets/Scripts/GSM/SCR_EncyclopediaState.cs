using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EncyclopediaState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EncyclopediaState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Encyclopedia clicked");
        //Show EncyclopediaUI
    }

    void IState.OnExit()
    {
        //Hide EncyclopediaUI
    }

    void IState.OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            stateMachine.ChangeState(stateMachine.MainMenuState);
            return;
        }
        Debug.Log("Showing Encyclopedia here");
    }
}
