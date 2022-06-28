using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SettingsState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    public Button Return;

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SettingsState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = true;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        Return = stateMachine.SettingsUI.transform.Find("Return").GetComponent<Button>();
        Return.onClick.AddListener(ReturnClicked);
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        Return.onClick.RemoveListener(ReturnClicked);
    }

    void IState.OnUpdate()
    {
        //do nothing yet
    }

    private void ReturnClicked()
    {
        stateMachine.ChangeState(stateMachine.MainMenuState);
    }
}
