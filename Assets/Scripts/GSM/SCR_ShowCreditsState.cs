using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ShowCreditsState :IState
{
    //Write attributes/properties here



    /// <summary>
    /// Constructor of state. Passes needed parameters into the state.
    /// </summary>
    public SCR_ShowCreditsState()
    {

    }

    void IState.OnEnter()
    {
        Debug.Log("Show credits clicked");
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnUpdate()
    {
        Debug.Log("Showing credits");
    }
}
