﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card")]
public class SO_Spell : ScriptableObject
{
    public Sprite cardImage;
    public int cardCost;
    public string cardDescription;
    public string cardName;
}
