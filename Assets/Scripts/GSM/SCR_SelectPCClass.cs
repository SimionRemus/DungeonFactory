using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SelectPCClass : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private Button shaman;
    private Button mage;
    private Button sorceress;
    private Button thaumaturge;
    private Button seer;
    private Button warlock;
    private Button druid;
    private Button randomClass;

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SelectPCClass(SCR_GameSM SM)
    {
        stateMachine = SM;
    }

    void IState.OnEnter()
    {
        Debug.Log("Entering PC class selection");

        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;
        stateMachine.ClassSelection.enabled = true;

        shaman = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Shaman").GetComponent<Button>();
        shaman.onClick.AddListener(ShamanClicked);

        mage = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Mage").GetComponent<Button>();
        mage.onClick.AddListener(MageClicked);

        sorceress = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Sorceress").GetComponent<Button>();
        sorceress.onClick.AddListener(SorceressClicked);

        thaumaturge = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Thaumaturge").GetComponent<Button>();
        thaumaturge.onClick.AddListener(ThaumaturgeClicked);

        seer = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Seer").GetComponent<Button>();
        seer.onClick.AddListener(SeerClicked);

        warlock = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Warlock").GetComponent<Button>();
        warlock.onClick.AddListener(WarlockClicked);

        druid = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("Druid").GetComponent<Button>();
        druid.onClick.AddListener(DruidClicked);

        randomClass = stateMachine.ClassSelection.transform.Find("ButtonsPanel").Find("RandomClass").GetComponent<Button>();
        randomClass.onClick.AddListener(RandomClicked);
    }
    void IState.OnExit()
    {
        shaman.onClick.RemoveListener(ShamanClicked);
        mage.onClick.RemoveListener(MageClicked);
        sorceress.onClick.RemoveListener(SorceressClicked);
        thaumaturge.onClick.RemoveListener(ThaumaturgeClicked);
        seer.onClick.RemoveListener(SeerClicked);
        warlock.onClick.RemoveListener(WarlockClicked);
        druid.onClick.RemoveListener(DruidClicked);
        randomClass.onClick.RemoveListener(RandomClicked);

        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;
        stateMachine.ClassSelection.enabled = false;
    }
    void IState.OnUpdate()
    {
        //Nothing to do here. Just waiting on button presses.
    }
    private void ShamanClicked()
    {
        AddPlayerStartingElements(elementType.Earth);
        AddPlayerStartingSpells(elementType.Earth);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void MageClicked()
    {
        AddPlayerStartingElements(elementType.Water);
        AddPlayerStartingSpells(elementType.Water);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void SorceressClicked()
    {
        AddPlayerStartingElements(elementType.Fire);
        AddPlayerStartingSpells(elementType.Fire);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void ThaumaturgeClicked()
    {
        AddPlayerStartingElements(elementType.Air);
        AddPlayerStartingSpells(elementType.Air);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void SeerClicked()
    {
        AddPlayerStartingElements(elementType.Divination);
        AddPlayerStartingSpells(elementType.Divination);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void WarlockClicked()
    {
        AddPlayerStartingElements(elementType.Illusion);
        AddPlayerStartingSpells(elementType.Illusion);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void DruidClicked()
    {
        AddPlayerStartingElements(elementType.Life);
        AddPlayerStartingSpells(elementType.Life);
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }
    private void RandomClicked()
    {
        int choice = UnityEngine.Random.Range(0, 7);
        switch (choice)
        {
            case 0:
                ShamanClicked();
                break;
            case 1:
                MageClicked();
                break;
            case 2:
                SorceressClicked();
                break;
            case 3:
                ThaumaturgeClicked();
                break;
            case 4:
                SeerClicked();
                break;
            case 5:
                WarlockClicked();
                break;
            case 6:
                DruidClicked();
                break;
            default:
                break;
        }

    }
    private void AddPlayerStartingElements(elementType elem)
    {
        stateMachine.player.GetComponent<SCR_Player>().GetNewInfusionElement(elem) ;
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots();
        stateMachine.player.GetComponent<SCR_Player>().GetNewInfusionElement(elem);
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots();
        stateMachine.player.GetComponent<SCR_Player>().GetNewInfusionElement(elem);
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots();
        stateMachine.player.GetComponent<SCR_Player>().GetNewInfusionElement(elem);
        stateMachine.player.GetComponent<SCR_Player>().RotateSlots();
    }

    private void AddPlayerStartingSpells(elementType elem)
    {
        List<GameObject> spellList = stateMachine.GetComponent<SCR_ObjectLists>().spellList;
        var parent = stateMachine.GameUI.transform.Find("ActionList");
        GameObject.Instantiate(spellList[0], parent);
        stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] =0;
        stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
        GameObject.Instantiate(spellList[2], parent);
        stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 2;
        stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
        GameObject.Instantiate(spellList[6], parent);
        stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 6;
        stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
        GameObject.Instantiate(spellList[8], parent);
        stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 8;
        stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
        switch (elem)
        {
            case elementType.None:
                break;
            case elementType.Earth:
                GameObject.Instantiate(spellList[11], parent);
                stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 11;
                stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
                GameObject.Instantiate(spellList[18], parent);
                stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 18;
                stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
                GameObject.Instantiate(spellList[19], parent);
                stateMachine.player.GetComponent<SCR_Player>().takenSpells[stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex] = 19;
                stateMachine.player.GetComponent<SCR_Player>().takenSpellsIndex++;
                break;
            case elementType.Water:
                break;
            case elementType.Fire:
                break;
            case elementType.Air:
                break;
            case elementType.Divination:
                break;
            case elementType.Illusion:
                break;
            case elementType.Life:
                break;
        }
    }

}
