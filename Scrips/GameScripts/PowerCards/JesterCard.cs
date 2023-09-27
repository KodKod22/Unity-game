using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterCard : AbilityManager
{
    CardDisplay card;
    public override void Ability()
    {
        base.Ability();
        
        if (BattleField.Instance.P1.deck.CardsInDeck.Count<=1)
        {
            DiscardManager.Instance.Movecardtodeck();
            card = BattleField.Instance.P1.deck.DrawCard();
            Debug.Log(" "+card.name);
            ChackCard();
        }
        else
        {
            card = BattleField.Instance.P1.deck.DrawCard();
            Debug.Log(" " + card.name);
            ChackCard();
        }
    }
    public void ChackCard()
    {
        if (card.card.type == cardType.number)
        {
            if (card.card.points % 2 == 0)
            {
                GetComponent<CardController>().Discard();
                AddCardToHand();
                if (TurnSystem.Instance.Isyourturn == false)
                {
                    foreach (CardDisplay c in BattleField.Instance.P1.CardsInHand)
                    {
                        c.GetComponent<CardController>().isClickable = false;
                    }
                    TurnSystem.Instance.UpdateTurn();
                }
            }
            else
            {
                GetComponent<CardController>().Discard();
                AddCardToHand();
                TurnSystem.Instance.EndTurn();

            }
        }
        else
        {
            GetComponent<CardController>().Discard();
            AddCardToHand();
            EndTurn();

        }
    }
    public void AddCardToHand()
    {
        if (TurnSystem.Instance.Isyourturn)
        {
            for (int i = 0; i < BattleField.Instance.P1.totalCardsInHand; i++)
            {
                if (BattleField.Instance.P1.handPositions[i].childCount == 0)
                {
                    CardController controller = card.GetComponent<CardController>();
                    RectTransform rect = card.GetComponent<RectTransform>();
                    rect.transform.SetParent(BattleField.Instance.P1.handPositions[i]);
                    rect.anchoredPosition = Vector2.zero;
                    rect.rotation = Quaternion.Euler(0, 0, 0);
                    BattleField.Instance.P1.CardsInHand.Add(card);
                    card.CardCover(false);
                    controller.playerController = BattleField.Instance.P1;
                    BattleField.Instance.P1.turnClick(true);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < BattleField.Instance.P2.totalCardsInHand; i++)
            {
                if (BattleField.Instance.P2.handPositions[i].childCount == 0)
                {
                    CardController controller = card.GetComponent<CardController>();
                    RectTransform rect = card.GetComponent<RectTransform>();
                    rect.transform.SetParent(BattleField.Instance.P2.handPositions[i]);
                    rect.anchoredPosition = Vector2.zero;
                    rect.rotation = Quaternion.Euler(0, 0, 0);
                    BattleField.Instance.P2.CardsInHand.Add(card);
                    card.CardCover(true);
                    controller.playerController = BattleField.Instance.P2;
                    BattleField.Instance.P2.turnClick(true);
                    break;
                }
            }
        }
        
    }
    
}
