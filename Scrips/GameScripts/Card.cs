using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCard", menuName = "card")]
public class Card : ScriptableObject
{
    public string nameCard;
    public int points;
    public cardType type;
    public Sprite icon;
    public int duplicateCount;
}

public enum cardType
{
    queen,
    king,
    dragon,
    knight,
    potion,
    wand,
    Jester,
    number
}
