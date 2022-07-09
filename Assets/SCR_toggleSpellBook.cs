using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_toggleSpellBook : MonoBehaviour
{
    [SerializeField]
    private GameObject spellbook;
    [SerializeField]
    private Image SelectionBoxSB;

   public void toggleSpellBook()
    {
        if(this.GetComponentInChildren<Text>().text== "Open Spellbook")
        {
            spellbook.SetActive(true);
            this.GetComponentInChildren<Text>().text = "Close Spellbook";
            
        }
        else
        {
            spellbook.SetActive(false);
            this.GetComponentInChildren<Text>().text = "Open Spellbook";
            SelectionBoxSB.enabled = false;
        }
    }
}
