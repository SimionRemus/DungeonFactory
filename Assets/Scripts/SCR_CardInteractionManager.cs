using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SCR_CardInteractionManager : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public static GameObject itemBeingSelected;
    Vector3 startPosition;
    [SerializeField] private Canvas CardDetailsUI;
    [SerializeField] private Canvas GameMenu;
    [SerializeField] private Image SelectionBox;

    private IEnumerator popUpCard;
    private GameObject prevGO=null;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("onPointerDown");
        Debug.Log(itemBeingSelected);
        if (itemBeingSelected == null || prevGO == itemBeingSelected)
        {
            itemBeingSelected = gameObject;
            SelectionBox.enabled = true;
            SelectionBox.rectTransform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            var CardDisplay=CardDetailsUI.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard = itemBeingSelected.GetComponent<SCR_CardInfoDisplay>().SpellCard;
            if (CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard != null)
            {
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardDescription.text = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardDescription;
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardName.text = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardName;
                CardDisplay.GetComponent<SCR_CardInfoDisplay>().Artwork.sprite = CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard.cardImage;
            }
            popUpCard = DoPopUP();
            StartCoroutine(popUpCard);
        }
        else
        {
            SelectionBox.enabled = false ;
            prevGO = itemBeingSelected;
            itemBeingSelected = null;
            var CardDisplay = CardDetailsUI.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().SpellCard = null;
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardDescription.text = "This is a card description placeholder.";
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().cardName.text = "CardName Placeholder";
            CardDisplay.GetComponent<SCR_CardInfoDisplay>().Artwork.sprite = null;
            CardDisplay.transform.position += new Vector3(0,-50, 0);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("onPointerUp");
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
