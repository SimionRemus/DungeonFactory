using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MainMenuStates : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_MainMenuStates(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Game started");
        //Should load MainMenuUI
    }

    void IState.OnExit()
    {
        //Should unload MainMenuUI
    }

    void IState.OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            // Change state LoadDataState (Continue Game)
            stateMachine.ChangeState(stateMachine.LoadDataState)
            return;
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            // Change state to Intro Cinematic (New Game)
            stateMachine.ChangeState(stateMachine.IntroCineState)
            return;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            //Change state to Settings
            stateMachine.ChangeState(stateMachine.SettingsState)
            return;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {   
            // Change state to Encyclopedia
            stateMachine.ChangeState(stateMachine.EncyclopediaState)
            return;
        }
        if(Input.GetKeyDown(KeyCode.B))
        {   
            // Change state to Show Credits
            stateMachine.ChangeState(stateMachine.ShowCreditsState)
            return;
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            Application.Quit();
            Debug.Log("Quitting the game has no effect in the Unity Editor so i'm giving this console message also.");
            return;
        }
        Debug.Log("Showing Main menu here");
    }
}
