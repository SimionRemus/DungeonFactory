using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NewSpellState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;

    private GameObject getNewSpell;
    private Button yesButton;
    private Button noButton;

    private Button swapSpellsButton;
    private Button cancelSwap;

    private GameObject selectedCardCC;
    private GameObject selectedCardSB;
    GameObject selectedCard;
    GameObject selectedSBCard;

    private string newSpellText = "Would you like to investigate your surroundings to perceive the magic threads around you and forge a new spell with them?";

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_NewSpellState(SCR_GameSM SM)
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

        getNewSpell = stateMachine.GameUI.transform.Find("GetNewSpellPanel").gameObject;
        getNewSpell.SetActive(true);

        getNewSpell.transform.Find("Text").GetComponent<Text>().text=newSpellText;

        yesButton = getNewSpell.transform.Find("Yes").GetComponent<Button>();
        yesButton.onClick.AddListener(YesAction);

        noButton = getNewSpell.transform.Find("No").GetComponent<Button>();
        noButton.onClick.AddListener(NoAction);

        swapSpellsButton = stateMachine.GameUI.transform.Find("Spellbook").Find("Swap Spells").GetComponent<Button>();
        swapSpellsButton.onClick.AddListener(SwapSpells);

        cancelSwap= stateMachine.GameUI.transform.Find("Spellbook").Find("CancelNewSpellSwap").GetComponent<Button>();
        cancelSwap.onClick.AddListener(CancelSwap);

        selectedCard = null;
        selectedSBCard = null;
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        yesButton.onClick.RemoveListener(YesAction);
        noButton.onClick.RemoveListener(NoAction);
        swapSpellsButton.onClick.RemoveListener(SwapSpells);
        cancelSwap.onClick.RemoveListener(CancelSwap);
        stateMachine.GameUI.transform.Find("Spellbook").Find("CancelNewSpellSwap").gameObject.SetActive(false);

        getNewSpell.SetActive(false);
    }

    void IState.OnUpdate()
    {
        selectedCardCC = stateMachine.GameUI.transform.Find("CardSelectionBoxCC").gameObject;
        Transform cardHolder = stateMachine.GameUI.transform.Find("GetNewSpellPanel").Find("CardChoice");
        for (int i = 0; i < cardHolder.childCount; i++)
        {
            var currentCard = cardHolder.transform.GetChild(i);
            if (currentCard.position == selectedCardCC.GetComponent<Image>().rectTransform.position)
            {
                selectedCard = currentCard.gameObject;
            }
        }
        if (stateMachine.GameUI.transform.Find("ActionList").childCount >= 10 && stateMachine.GameUI.transform.Find("Spellbook").Find("Cards").childCount >= 20)
        {
            selectedCardSB = stateMachine.GameUI.transform.Find("CardSelectionBoxSB").gameObject;
            if (selectedCardSB)
            {
                Transform spellbook = stateMachine.GameUI.transform.Find("Spellbook").Find("Cards");
                for (int i = 0; i < spellbook.childCount; i++)
                {
                    var currentCard = spellbook.transform.GetChild(i);
                    if (currentCard.position == selectedCardSB.GetComponent<Image>().rectTransform.position)
                    {
                        selectedSBCard = currentCard.gameObject;
                    }
                }
            }
        }
    }

    private void YesAction()
    {
        //Check if there is willpower left to add spell
        if (stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower >=1)
        {
            //Add spells
            if(selectedCard)
            {
                if(stateMachine.GameUI.transform.Find("ActionList").childCount<10)
                {
                    GameObject newSpell=GameObject.Instantiate(selectedCard);
                    newSpell.name = selectedCard.name;
                    newSpell.transform.SetParent(stateMachine.GameUI.transform.Find("ActionList"));
                    newSpell.transform.SetSiblingIndex(stateMachine.GameUI.transform.Find("ActionList").childCount-1);

                    selectedCardCC.transform.GetComponent<Image>().enabled = false;
                    //End turn
                    stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower -= 1;
                    stateMachine.ChangeState(stateMachine.EnvironmentTurnState);
                }
                else
                {
                    if (stateMachine.GameUI.transform.Find("Spellbook").Find("Cards").childCount < 20)
                    {
                        GameObject newSpell = GameObject.Instantiate(selectedCard);
                        newSpell.name = selectedCard.name;
                        newSpell.transform.SetParent(stateMachine.GameUI.transform.Find("Spellbook").Find("Cards"));
                        newSpell.transform.SetSiblingIndex(stateMachine.GameUI.transform.Find("Spellbook").Find("Cards").childCount - 1);

                        selectedCardCC.transform.GetComponent<Image>().enabled = false;
                        //End turn
                        stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower -= 1;
                        stateMachine.ChangeState(stateMachine.EnvironmentTurnState);
                    }
                    else
                    {
                        //Ask player to replace a spell from spellbook.
                        stateMachine.GameUI.transform.Find("Spellbook").gameObject.SetActive(true);
                        stateMachine.GameUI.transform.Find("Spellbook").Find("CancelNewSpellSwap").gameObject.SetActive(true);
                        getNewSpell.SetActive(false);
                        selectedCardCC.transform.GetComponent<Image>().enabled = false;
                    }
                }
                
            }            
        }      
    }

    private void NoAction()
    {
        selectedCardCC.transform.GetComponent<Image>().enabled = false;
        stateMachine.ChangeState(stateMachine.EnvironmentTurnState);
    }

    private void CancelSwap()
    {
        selectedCardCC.transform.GetComponent<Image>().enabled = false;
        stateMachine.GameUI.transform.Find("Spellbook").gameObject.SetActive(false);
        selectedCardSB.transform.GetComponent<Image>().enabled = false;
        stateMachine.ChangeState(stateMachine.EnvironmentTurnState);
    }

    private void SwapSpells()
    {
        if (selectedSBCard)
        {
            Debug.Log("Entered Swap Spells");
            int a = selectedSBCard.transform.GetSiblingIndex();
            GameObject.Destroy(selectedSBCard);
            GameObject newSpell = GameObject.Instantiate(selectedCard);
            newSpell.name = selectedCard.name;
            newSpell.transform.SetParent(stateMachine.GameUI.transform.Find("Spellbook").Find("Cards"));
            newSpell.transform.SetSiblingIndex(a);

            stateMachine.GameUI.transform.Find("Spellbook").gameObject.SetActive(false);
            selectedCardSB.transform.GetComponent<Image>().enabled = false;
            //End turn
            stateMachine.player.GetComponent<SCR_Player>().numberOfWillpower -= 1;
            stateMachine.ChangeState(stateMachine.EnvironmentTurnState);
        }  
    }
}
