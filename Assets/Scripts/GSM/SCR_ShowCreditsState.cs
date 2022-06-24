using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ShowCreditsState :IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_ShowCreditsState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Show credits clicked");
        //Load and show credits as video or marching text or whatever
    }

    void IState.OnExit()
    {
        //Unload or hide anything needed in this state.
    }

    void IState.OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            stateMachine.ChangeState(stateMachine.MainMenuState)
            return;
        }
        Debug.Log("Showing credits");
    }
}
