using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_CardBodyTint : MonoBehaviour
{
    elementType mandatoryElement;
    Color backColor;

    // Start is called before the first frame update
    void Awake()
    {
        mandatoryElement = this.transform.parent.GetComponent<SCR_CardInfoDisplay>().SpellCard.mandatoryElement;
        backColor = GetComponent<Image>().color;
        switch (mandatoryElement)
        {
            case elementType.None:
                backColor = new Color32(0xB0,0xB0,0xB0, 0xFF);
                break;
            case elementType.Earth:
                backColor = new Color32(0xA2, 0x70, 0x40, 0xFF);
                break;
            case elementType.Water:
                backColor = new Color32(0x70, 0x90, 0xD0, 0xFF);
                break;
            case elementType.Fire:
                backColor = new Color32(0xAB, 0x30, 0x00, 0xFF);
                break;
            case elementType.Air:
                backColor = new Color32(0xFC, 0xFF, 0xD0,0xFF);
                break;
            case elementType.Divination:
                backColor = new Color32(0x0C, 0xA0, 0xA0, 0xFF);
                break;
            case elementType.Illusion:
                backColor = new Color32(0xFF, 0xA0, 0x50, 0xFF);
                break;
            case elementType.Life:
                backColor = new Color32(0x18, 0x70, 0x00, 0xFF);
                break;
        }
    }
}
