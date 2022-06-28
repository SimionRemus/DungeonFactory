using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerTurnIdleState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;


    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_PlayerTurnIdleState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = true;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;
    }

    void IState.OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            Debug.Log("Pressed escape. returning to main menu. This is for debug purposes only!!!");
            stateMachine.ChangeState(stateMachine.MainMenuState);
            return;
        }
    }
}
