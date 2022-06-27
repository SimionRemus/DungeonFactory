using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_IntroCinematicState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_IntroCinematicState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Entering Intro Cinematic");
        //Show intro cinematic for new game
    }

    void IState.OnExit()
    {
        //Unload/hide anything from the cinematic, if needed
    }

    void IState.OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            stateMachine.ChangeState(stateMachine.MainMenuState);
            return;
        }
        Debug.Log("Showing Intro Cinematics here");
    }
}

