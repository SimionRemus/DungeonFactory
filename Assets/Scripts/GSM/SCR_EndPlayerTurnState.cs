using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SCR_EndPlayerTurnState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;

    private GameObject getElementPanel;
    private Button yesButton;
    private Button noButton;

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_EndPlayerTurnState(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Ending player turn");

        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = true;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        getElementPanel=stateMachine.GameUI.transform.Find("GetElementPanel").gameObject;
        getElementPanel.SetActive(true);

        yesButton =getElementPanel.transform.Find("Yes").GetComponent<Button>();
        yesButton.onClick.AddListener(YesAction);

        noButton = getElementPanel.transform.Find("No").GetComponent<Button>();
        noButton.onClick.AddListener(NoAction);
    }

    void IState.OnExit()
    {
        Debug.Log("Ending enemy turn for debug purposes");

        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        getElementPanel.SetActive(false);
        yesButton.onClick.RemoveListener(YesAction);
        noButton.onClick.RemoveListener(NoAction);
    }

    void IState.OnUpdate()
    {
        //stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }

    private void YesAction()
    {
        stateMachine.ChangeState(stateMachine.UpgradeSpellState);
    }

    private void NoAction()
    {
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots();
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState); //will be enemy turn in the end
    }

}