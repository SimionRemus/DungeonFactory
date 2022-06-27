using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_MainMenuStates : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    public Button ContinueGame;
    public Button NewGame;
    public Button Settings;
    public Button Encyclopedia;
    public Button Credits;
    public Button Quit;
    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_MainMenuStates(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        stateMachine.MainMenuUI.enabled = true;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        Debug.Log("Game started");
        //Buttons assigned
        ContinueGame = stateMachine.MainMenuUI.transform.Find("ContinueGame").GetComponent<Button>();
        NewGame = stateMachine.MainMenuUI.transform.Find("New Game").GetComponent<Button>();
        Settings = stateMachine.MainMenuUI.transform.Find("Settings").GetComponent<Button>();
        Encyclopedia = stateMachine.MainMenuUI.transform.Find("Encyclopedia").GetComponent<Button>();
        Credits = stateMachine.MainMenuUI.transform.Find("Credits").GetComponent<Button>();
        Quit = stateMachine.MainMenuUI.transform.Find("Quit").GetComponent<Button>();
        //Button listeners assigned
        ContinueGame.onClick.AddListener(ContGameClicked);
        NewGame.onClick.AddListener(NewGameClicked);
        Settings.onClick.AddListener(SettingsClicked);
        Encyclopedia.onClick.AddListener(EncyclopediaClicked);
        Credits.onClick.AddListener(CreditsClicked);
        Quit.onClick.AddListener(QuitClicked);

    }



    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;

        ContinueGame.onClick.RemoveListener(ContGameClicked);
        NewGame.onClick.RemoveListener(NewGameClicked);
        Settings.onClick.RemoveListener(SettingsClicked);
        Encyclopedia.onClick.RemoveListener(EncyclopediaClicked);
        Credits.onClick.RemoveListener(CreditsClicked);
        Quit.onClick.RemoveListener(QuitClicked);
    }

    void IState.OnUpdate()
    {
        //Nothing happens here.
    }

    private void ContGameClicked()
    {
        stateMachine.ChangeState(stateMachine.LoadDataState);
    }

    private void NewGameClicked()
    {
        stateMachine.ChangeState(stateMachine.IntroCineState);
    }

    private void SettingsClicked()
    {
        stateMachine.ChangeState(stateMachine.SettingsState);
    }

    private void EncyclopediaClicked()
    {
        stateMachine.ChangeState(stateMachine.EncyclopediaState);
    }

    private void CreditsClicked()
    {
        stateMachine.ChangeState(stateMachine.ShowCreditsState);
    }

    private void QuitClicked()
    {
        Application.Quit();
        Debug.Log("Quitting the game has no effect in the Unity Editor so i'm giving this console message also.");
    }
}
