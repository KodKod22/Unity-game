using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCard : AbilityManager
{
    CardDisplay queenCard;
    public override void Ability()
    {
        base.Ability();
        foreach (CardDisplay card in BattleField.Instance.CardsInSlot)
        {
            card.GetComponent<QueenCard>().SetKingCard(this);
            card.GetComponent<CardController>().isClickable = true;
            card.GetComponent<CardController>().playerController = cardController.playerController;
        }
        if (!TurnSystem.Instance.Isyourturn && BattleField.Instance.P2.GetComponent<AIPlayer>())
        {
            for (int i = 0 ; i < BattleField.Instance.totlQueenslot ; i++)
            {
                if (BattleField.Instance.CardsInSlot[i].gameObject.activeSelf == true)
                {
                    BattleField.Instance.CardsInSlot[i].GetComponentInChildren<CardController>().InvokeAbility();
                    Debug.Log("AI picking random queen!");
                    break;
                }
                else
                {
                    continue;
                }
            }
            return;
        }
        
        return;
    }

    public void SetQueenCard(CardDisplay card)
    {
        queenCard = card;
        foreach (CardDisplay c in BattleField.Instance.CardsInSlot)
        {
            c.GetComponent<CardController>().isClickable = false;
        }
        if(TurnSystem.Instance.Isyourturn)
        {
            BattleField.Instance.P1.AddQueen(card.GetComponent<QueenCard>());
            GetComponent<CardController>().Discard();
            EndTurn();
            return;
        }
            BattleField.Instance.P2.AddQueen(card.GetComponent<QueenCard>());
            GetComponent<CardController>().Discard();
            EndTurn();
        return;
    }
}
