using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// OnEnter is run whenever you enter the state
    /// </summary>
    void OnEnter();

    /// <summary>
    /// OnUpdate is run periodically (simulates the Update method without Monobehaviour attacheds).
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// OnExit is run whenever you exit the state
    /// </summary>
    void OnExit();

}
