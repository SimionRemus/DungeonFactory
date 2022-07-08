using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_CardInfoDisplay : MonoBehaviour
{
    public SO_Spell SpellCard;
    public Image Artwork;
    public Text cardDescription;
    public Text cardName;
    public Image cardBody;

    // Start is called before the first frame update
    void Start()
    {
        if (SpellCard != null)
        {
            cardDescription.text = SpellCard.cardDescription;
            cardName.text = SpellCard.cardName;
            Artwork.sprite = SpellCard.cardImage;
        }
    }

}
