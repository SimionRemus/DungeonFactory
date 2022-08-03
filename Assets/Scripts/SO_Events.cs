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
    public EventStability stability;

    public void DoEffect()
    {
        Debug.Log(this.eventType);
    }
}

public enum EventStability
{
    Stable,
    Unstable,
    Volatile
}

public enum TileEventTypes
{
    AlterEgo,
    ShopLifter,
    ApothecaryVisit,
    MagicumLaude,
    ElementalAffinity,
    Overmind,
    ThinkingWithGateways,
    ItsMySweat,
    AndBlood,
    Goldfish,
    DarkBlood,
    Dizziness,
    Eraser,
    TheNoctisReaper,
    MagicStew,
    WetMatches,
    ElementalDissonance
}