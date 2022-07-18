using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SpellBookSwapState : IState
{
    //Write attributes/properties here
    private SCR_GameSM stateMachine;
    private GameObject spellbook;
    private Image SelectionBoxSB;
    private Image SelectionBox;
    public Button spellbookButton;
    private Button SwapSpellsButton;

    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_SpellBookSwapState(SCR_GameSM SM)
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


        spellbook = stateMachine.GameUI.transform.Find("Spellbook").gameObject;

        Debug.Log("Opening spellbook");
        spellbook.SetActive(true);
        stateMachine.GameUI.transform.Find("OpenSpellbook").GetComponentInChildren<Text>().text = "Close Spellbook";

        SelectionBoxSB = stateMachine.GameUI.transform.Find("CardSelectionBoxSB").gameObject.GetComponent<Image>();
        SelectionBox = stateMachine.GameUI.transform.Find("CardSelectionBox").gameObject.GetComponent<Image>();

        spellbookButton = stateMachine.GameUI.transform.Find("OpenSpellbook").GetComponent<Button>();
        spellbookButton.onClick.AddListener(SpellbookClicked);

        spellbookButton = stateMachine.GameUI.transform.Find("Spellbook").Find("Swap Spells").GetComponent<Button>();
        spellbookButton.onClick.AddListener(SwapCards);
    }

    void IState.OnExit()
    {
        stateMachine.MainMenuUI.enabled = false;
        stateMachine.SettingsUI.enabled = false;
        stateMachine.GameUI.enabled = false;
        stateMachine.CardDetailsUI.enabled = false;
        stateMachine.CreditsUI.enabled = false;
        stateMachine.IntroCinematics.enabled = false;

        Debug.Log("Closing spellbook");
        spellbook.SetActive(false);
        stateMachine.GameUI.transform.Find("OpenSpellbook").GetComponentInChildren<Text>().text = "Open Spellbook";
        SelectionBoxSB.enabled = false;

        spellbookButton.onClick.RemoveListener(SpellbookClicked);
        spellbookButton.onClick.RemoveListener(SwapCards);
    }

    void IState.OnUpdate()
    {
        //do the card swap.
    }

    private void SpellbookClicked()
    {
        stateMachine.ChangeState(stateMachine.PlayerTurnIdleState);
    }

    private void SwapCards()
    {
        GameObject actionlist = stateMachine.GameUI.transform.Find("ActionList").gameObject;
        int cardnumberKnown = stateMachine.GameUI.transform.Find("ActionList").childCount;
        int cardNumberBook = spellbook.transform.Find("Cards").childCount;
        GameObject selectedCardKnown=null;
        GameObject selectedCardBook=null;

        for (int i = 0; i < cardnumberKnown; i++)
        {
            var currentCard = actionlist.transform.GetChild(i);
            if (currentCard.position == SelectionBox.rectTransform.position)
            {
                selectedCardKnown = currentCard.gameObject;
                Debug.Log(selectedCardKnown.name);
            }
        }
        for (int i = 0; i < cardNumberBook; i++)
        {
            var currentCard = spellbook.transform.Find("Cards").GetChild(i);
            if (currentCard.position == SelectionBoxSB.rectTransform.position)
            {
                selectedCardBook = currentCard.gameObject;
                Debug.Log(selectedCardBook.name);
            }
        }
        if(selectedCardKnown!=null && selectedCardBook!=null)
        {
            int selectedCardKnownIndex=selectedCardKnown.transform.GetSiblingIndex();
            int selectedCardBookIndex = selectedCardBook.transform.GetSiblingIndex();

            GameObject swappedKnown= GameObject.Instantiate(selectedCardKnown);
            swappedKnown.transform.SetParent(selectedCardBook.transform.parent);
            swappedKnown.transform.SetSiblingIndex(selectedCardBookIndex);

            GameObject swappedBook = GameObject.Instantiate(selectedCardBook);
            swappedBook.transform.SetParent(selectedCardKnown.transform.parent);
            swappedBook.transform.SetSiblingIndex(selectedCardKnownIndex);

            GameObject.Destroy(selectedCardKnown);
            GameObject.Destroy(selectedCardBook);
        }
    }

}
