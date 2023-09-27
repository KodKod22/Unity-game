using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class knightCard : AbilityManager
{
    QueenCard queenCard;
    public override void Ability()
    {
        base.Ability();
        if (TurnSystem.Instance.Isyourturn)
        {
            if (BattleField.Instance.P2.TotalQueens.Count == 0)
            {
                GetComponent<CardController>().Discard();
                EndTurn();
                return;

            }
            queenCard = BattleField.Instance.P2.TotalQueens.Last();
            queenCard.GetComponent<CardController>().playerController = cardController.playerController;
            BattleField.Instance.P2.RemoveQuenn(queenCard);
            BattleField.Instance.P1.AddQueen(queenCard);
            GetComponent<CardController>().Discard();
            EndTurn();
            return;
        }
        else if (!TurnSystem.Instance.Isyourturn)
        {
            if (BattleField.Instance.P1.TotalQueens.Count == 0)
            {
                GetComponent<CardController>().Discard();
                EndTurn();
                return;
            }
            queenCard = BattleField.Instance.P1.TotalQueens.Last();
            queenCard.GetComponent<CardController>().playerController = cardController.playerController;
            BattleField.Instance.P1.RemoveQuenn(queenCard);
            BattleField.Instance.P2.AddQueen(queenCard);
            GetComponent<CardController>().Discard();
            EndTurn();
        }
    }
}
