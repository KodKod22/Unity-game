using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AIPlayer : PlayerController
{
    [Header("AI params")]
    [SerializeField] CardDisplay playingCard;
    int randomMove;
   
public void MakeMove()
    {

        if (CardsInHand.Count > 0)
        {
            Invoke(nameof(DoTheMove), 0.5f);
            return;
        }
        else
        {
            if (deck.CardsInDeck.Count == 0)
            {
                DiscardManager.Instance.Movecardtodeck();
                StartCoroutine(DrawCardsAfterFinishDeck());
            }
            else
            {
                InitHand();
            }
        }
    }
    public void DoTheMove()
    {
        randomMove = UnityEngine.Random.Range(0, CardsInHand.Count);
        playingCard = CardsInHand[randomMove];
        StartCoroutine(RevelTheCard());
    }

    public IEnumerator RevelTheCard()
    {
        playingCard.CardCover(false);
        yield return new WaitForSeconds(1.5f);
        playthecard();

        yield break;
    }
    public void playthecard()
    {
        playingCard.GetComponent<CardController>().ActivateCard();
    }

    public IEnumerator DrawCardsAfterFinishDeck()
    {
        yield return new WaitUntil(() => deck.isDoneShuffle);
        InitHand();
        yield return new WaitUntil(() => CardsInHand.Count > 0);
        MakeMove();
        yield break;
    }

}
