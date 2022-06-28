using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GameSM : MonoBehaviour
{
    public IState currentState;
    bool intransition = false;

    #region States definition
    public SCR_MainMenuStates MainMenuState;
    public SCR_SettingsState SettingsState;
    public SCR_ShowCreditsState ShowCreditsState;
    public SCR_CastSpellState CastSpellState;
    public SCR_IntroCinematicState IntroCineState;
    public SCR_DefeatState DefeatState;
    public SCR_DoEventTileState DoEventTileState;
    public SCR_EncyclopediaState EncyclopediaState;
    public SCR_EndPlayerTurnState EndPlayerTurnState;
    public SCR_EnvironmentTurnState EnvironmentTurnState;
    public SCR_LoadDataState LoadDataState;
    public SCR_PlayerTurnIdleState PlayerTurnIdleState;
    public SCR_SaveDataState SaveDataState;
    public SCR_UpgradeSpellState UpgradeSpellState;
    public SCR_VictoryState VictoryState;
    #endregion

    [SerializeField] public Canvas MainMenuUI;
    [SerializeField] public Canvas SettingsUI;
    [SerializeField] public Canvas GameUI;
    [SerializeField] public Canvas CardDetailsUI;
    [SerializeField] public Canvas CreditsUI;
    [SerializeField] public Canvas IntroCinematics;
    public void Start()
    {
        #region State initialization
        MainMenuState = new SCR_MainMenuStates(this);
        SettingsState = new SCR_SettingsState(this);
        ShowCreditsState = new SCR_ShowCreditsState(this);
        CastSpellState = new SCR_CastSpellState(this);
        IntroCineState = new SCR_IntroCinematicState(this);
        DefeatState = new SCR_DefeatState(this);
        DoEventTileState = new SCR_DoEventTileState(this);
        EncyclopediaState = new SCR_EncyclopediaState(this);
        EndPlayerTurnState = new SCR_EndPlayerTurnState(this);
        EnvironmentTurnState = new SCR_EnvironmentTurnState(this);
        LoadDataState = new SCR_LoadDataState(this);
        PlayerTurnIdleState = new SCR_PlayerTurnIdleState(this);
        SaveDataState = new SCR_SaveDataState(this);
        UpgradeSpellState = new SCR_UpgradeSpellState(this);
        VictoryState = new SCR_VictoryState(this);
        #endregion
        ChangeState(MainMenuState);
    }

    public void ChangeState(IState newState)
    {
        if (currentState == newState || intransition)
            return;
        ChangeStateRoutine(newState);
    }

    void ChangeStateRoutine(IState newState)
    {
        intransition = true;

        if (currentState != null)
            currentState.OnExit();
        
        currentState = newState;

        if (currentState != null)
            currentState.OnEnter();

        intransition = false;
    }

    public void Update()
    {
        if(currentState!=null && !intransition)
            currentState.OnUpdate();
    }
}
