using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SCR_GameSM : MonoBehaviour
{
    public IState currentState;
    public IState previousState;
    bool intransition = false;
    public void ChangeState(IState newState)
    {
        if (currentState == newState || intransition)
            return;
        ChangeStateRoutine(newState);
    }

    public void RevertState()
    {
        if (previousState != null)
            ChangeState(previousState);
    }

    void ChangeStateRoutine(IState newState)
    {
        intransition = true;

        if (currentState != null)
            currentState.OnExit();
        if (previousState != null)
            previousState = currentState;

        currentState = newState;

        if (currentState != null)
            currentState.OnEnter();

        intransition = false;
    }

    public void Update()
    {
        if(currentState!=null && !intransition)
            currentState.OnUpdate();
    }
}
