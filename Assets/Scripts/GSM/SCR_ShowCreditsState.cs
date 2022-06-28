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
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = true;
        stateMachine.CreditsUI.transform.Find("credits").GetComponent<Animator>().Play("creditsAnim",-1,0.0f);
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Change state MainMenu (Main Menu)
            stateMachine.ChangeState(stateMachine.MainMenuState);
            return;
        }
        Debug.Log("Showing credits");
    }
}
