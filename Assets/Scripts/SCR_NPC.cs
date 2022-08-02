using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_NPC : MonoBehaviour
{
    public SO_NPC npcData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(npcData.HP<=0)
        {
            //NPC DIES
            GameObject.Destroy(this.gameObject);
        }
    }

    public void ApplyEffects()
    {

    }

    public void AffectHitpoints(int ammount)
    {
        if (npcData.HP + ammount > npcData.HPMax)
        {
            npcData.HP = npcData.HPMax;
        }
        else
        {
            if (npcData.HP + ammount < 0)
                npcData.HP = 0;
            else
                npcData.HP += ammount;
        }
    }
}
