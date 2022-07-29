using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Events", menuName = "ScriptableObjects/Events/Event")]
public class SO_Events : ScriptableObject
{
    public TileEventTypes eventType;
    public int probabilityWeight;

    public void DoEffect()
    {
        Debug.Log(this.eventType);
    }
}


public enum TileEventTypes
{
    GenericEvent1,
    GenericEvent2
}