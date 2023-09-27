using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PotionCard : AbilityManager
{
    public QueenCard queen;
    public override void Ability()
    {
        base.Ability();

        if (!TurnSystem.Instance.Isyourturn)
        {
            if (BattleField.Instance.P1.TotalQueens.Count == 0)
            {
                GetComponent<CardController>().Discard();
                EndTurn();
            }
            else
            {
                queen = BattleField.Instance.P1.TotalQueens.Last();
                BattleField.Instance.placeQueens(queen.GetComponent<CardDisplay>());
                BattleField.Instance.P1.RemoveQuenn(queen);
                GetComponent<CardController>().Discard();
                EndTurn();
            }
         
        }
        else
        {
            if (BattleField.Instance.P2.TotalQueens.Count == 0)
            {
                GetComponent<CardController>().Discard();
                EndTurn();
            }
            else
            {
                queen = BattleField.Instance.P2.TotalQueens.Last();
                Debug.Log("stole queen to field!!!!!! " + queen);
                BattleField.Instance.placeQueens(queen.GetComponent<CardDisplay>());
                BattleField.Instance.P2.RemoveQuenn(queen);
                
                GetComponent<CardController>().Discard();
                EndTurn();
                
            }
        }
    }
}
