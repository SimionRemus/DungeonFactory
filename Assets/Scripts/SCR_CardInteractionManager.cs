using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SCR_CardInteractionManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static GameObject itemBeingSelected;
    Vector3 startPosition;
    [SerializeField] private Canvas CardDetailsUI;
    [SerializeField] private Canvas GameMenu;
    [SerializeField] private Image SelectionBox;
    [SerializeField] private Image SelectionBoxSB;

    private IEnumerator popUpCard;
    private GameObject prevGO=null;
    private int cardCount;
    private Transform actionList;
    private Transform spellBook;

    #region ColorDefinition
    Color Earth, Water, Air, Fire, Divination, Illusion, Life, NoElement;
    #endregion


    private void Start()
    {
        #region ColorInstantiation
        Earth= new Color32(0x85, 0x55, 0x30, 0xFF);
        Water= new Color32(0x3E, 0x7B, 0xF4, 0xFF);
        Air = new Color32(0xFC, 0xFF, 0xD0, 0xFF);
        Fire = new Color32(0xAB, 0x30, 0x00, 0xFF);
        Divination = new Color32(0x90, 0x00, 0xA0, 0xFF);
        Illusion = new Color32(0xFF, 0xA0, 0x50, 0xFF);
        Life = new Color32(0x18, 0x70, 0x00, 0xFF);
        NoElement = new Color32(0x80, 0x80, 0x80, 0xFF);
        #endregion
        #region Get CardDetailsUI,GameUI and CardSelectionBox to use in OnPointerDown/UP
        var UIS = GameObject.FindGameObjectsWithTag("UIs");
        foreach (var ui in UIS)
        {
            if (ui.ToString().Split(' ')[0].Equals("CardDetailsUI"))
            {
                CardDetailsUI = ui.GetComponent<Canvas>();
            }
            if (ui.ToString().Split(' ')[0].Equals("GameUI"))
            {
                GameMenu = ui.GetComponent<Canvas>();
            }
        }
        var cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (var ui in cards)
        {
            if (ui.ToString().Split(' ')[0].Equals("CardSelectionBox"))
            {
                SelectionBox = ui.GetComponent<Image>();
            }
            if (ui.ToString().Split(' ')[0].Equals("CardSelectionBoxSB"))
            {
                SelectionBoxSB = ui.GetComponent<Image>();
            }
        }
        #endregion
        #region Add Color based on elements
        actionList = GameMenu.transform.Find("ActionList");
        cardCount = actionList.childCount;
        for (int i = 0; i < cardCount; i++)
        {
            var mandatoryElement = actionList.GetChild(i).GetComponent<SCR_CardInfoDisplay>().SpellCard.mandatoryElement;
            switch (mandatoryElement)
            {
                case elementType.None:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = NoElement;
                    break;
                case elementType.Earth:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Earth;
                    break;
                case elementType.Water:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Water;
                    break;
                case elementType.Fire:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Fire;
                    break;
                case elementType.Air:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Air;
                    break;
                case elementType.Divination:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Divination;
                    break;
                case elementType.Illusion:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Illusion;
                    break;
                case elementType.Life:
                    actionList.GetChild(i).GetChild(0).GetComponent<Image>().color = Life;
                    break;
            }
        }
        spellBook = GameMenu.transform.Find("Spellbook");
        cardCount = spellBook.childCount;
        for (int i = 0; i < cardCount; i++)
        {
            var mandatoryElement = spellBook.GetChild(i).GetComponent<SCR_CardInfoDisplay>().SpellCard.mandatoryElement;
            switch (mandatoryElement)
            {
                case elementType.None:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = NoElement;
                    break;
                case elementType.Earth:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Earth;
                    break;
                case elementType.Water:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Water;
                    break;
                case elementType.Fire:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Fire;
                    break;
                case elementType.Air:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Air;
                    break;
                case elementType.Divination:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Divination;
                    break;
                case elementType.Illusion:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Illusion;
                    break;
                case elementType.Life:
                    spellBook.GetChild(i).GetChild(0).GetComponent<Image>().color = Life;
                    break;
            }
        }
        #endregion
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemBeingSelected == null || prevGO == itemBeingSelected)
        {
            itemBeingSelected = gameObject;
            if (itemBeingSelected.transform.parent == actionList)
            {
                SelectionBox.enabled = true;
                SelectionBox.rectTransform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            }
            else
            {
                SelectionBoxSB.enabled = true;
                SelectionBoxSB.rectTransform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            }
            prevGO = itemBeingSelected;
            //SelectionBox.rectTransform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            var CardDisplay = CardDetailsUI.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard = itemBeingSelected.GetComponent<SCR_CardInfoDisplay>().SpellCard;
            if (CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard != null)
            {
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardDescription.text = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardDescription;
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardName.text = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardName;
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().Artwork.sprite = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardImage;
                CardDisplay.transform.GetChild(0).GetComponent<Image>().color = itemBeingSelected.transform.GetChild(0).GetComponent<Image>().color;
            }
            popUpCard = DoPopUP();
            StartCoroutine(popUpCard);
        }
        else
        {
            SelectionBox.enabled = false;
            SelectionBoxSB.enabled = false;
            itemBeingSelected = null;
            var CardDisplay = CardDetailsUI.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard = null;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardDescription.text = "This is a card description placeholder.";
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardName.text = "CardName Placeholder";
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().Artwork.sprite = null;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardBody.color = Color.white;
            CardDisplay.transform.position += new Vector3(0, 0, 0);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (popUpCard != null) 
        {
            StopCoroutine(popUpCard);
        }
        CardDetailsUI.enabled = false;
        GameMenu.enabled = true;
        
    }

    private IEnumerator DoPopUP()
    {
        yield return new WaitForSeconds(0.5f);
        CardDetailsUI.enabled = true;
        GameMenu.enabled = false;
    }
}
