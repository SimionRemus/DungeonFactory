using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NPC : MonoBehaviour
{
    [SerializeField]
    private int HP;
    private int HPMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP<=0)
        {
            //NPC DIES
        }
    }

    public void ApplyEffects()
    {

    }

    public void AffectHitpoints(int ammount)
    {
        if (HP + ammount > HPMax)
        {
            HP = HPMax;
        }
        else
        {
            if (HP + ammount < 0)
                HP = 0;
            else
                HP += ammount;
        }
    }
}
