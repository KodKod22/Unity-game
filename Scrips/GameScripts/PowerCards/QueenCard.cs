using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenCard : AbilityManager
{
     public KingCard kingCard;
    public override void Ability()
    {
        base.Ability();
        kingCard.SetQueenCard(GetComponent<CardDisplay>());
        gameObject.SetActive(false);
    }
    public void SetKingCard(KingCard _kingCard)
    {
        kingCard = _kingCard;
    }
}
