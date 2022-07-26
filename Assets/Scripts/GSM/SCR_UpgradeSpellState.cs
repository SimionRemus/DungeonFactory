using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Threading;

public class SCR_UpgradeSpellState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private bool coroutineStarted;


    private IEnumerator waitCoroutineResponse;
    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_UpgradeSpellState(SCR_GameSM SM)
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

        coroutineStarted = false;
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        stateMachine.StopCoroutine(waitCoroutineResponse);
    }

    void IState.OnUpdate()
    {
        Vector3 playerPos = stateMachine.player.transform.position;
        Tilemap gndTilemap = stateMachine.grid.transform.Find("GroundTilemap").GetComponent<Tilemap>();
        Vector3Int tilePos = gndTilemap.WorldToCell(playerPos);
        TileWithAttributes currentTile = (TileWithAttributes)gndTilemap.GetTile(tilePos);
        elementType elem = currentTile.element;
        stateMachine.player.GetComponent<SCR_Player>().GetNewInfusionElement(elem);
        if (!coroutineStarted)
        {
            waitCoroutineResponse = WAIT(1f);
            stateMachine.StartCoroutine(waitCoroutineResponse);
            coroutineStarted = true;
        }
    }

    private IEnumerator WAIT(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots(); //will be enemy turn in the end
        stateMachine.ChangeState(stateMachine.NewSpellState); //will be enemy turn in the end
    }
}
