﻿using System.Collections;
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
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = true;

        
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        //GENERATE NEW GAME HERE (generate floor, events, monsters, etc; reset position, element infusions, spells, torches, etc for PC; etc)
        stateMachine.grid.GetComponent<SCR_FloorGeneration>().GenerateWorld();
        stateMachine.player.GetComponent<SCR_Player>().ResetPlayerState();
    }

    void IState.OnUpdate()
    {
        //running cinematic video then going into new game
        stateMachine.ChangeState(stateMachine.SelectPCClass);

    }
}

