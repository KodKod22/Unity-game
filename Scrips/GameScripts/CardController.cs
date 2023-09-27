using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour , IPointerClickHandler
{
    public Action SpecialAbility;
    public PlayerController playerController;
    public AIPlayer Ai;



    public bool isClickable=false;
    private void Start()
    {
        
        foreach (CardDisplay card in BattleField.Instance.CardsInSlot)
        {
            card.GetComponent<CardController>().isClickable = false;
        }

    }


    public virtual void Discard()
    {
        playerController.CardsInHand.Remove(this.gameObject.GetComponent<CardDisplay>());
        
        DiscardManager.Instance.MoveCardToDiscardList(this.gameObject.GetComponent<CardDisplay>());

    }
   
   
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (isClickable &&Sc_GameLogic.Instance.Isyourturn && BattleField.Instance.isGameOver == false)
        //{
        //    Sc_GameLogic.Instance.SendMove(this.GetComponent<CardDisplay>());
        //    Sc_GameLogic.Instance.Placement(this.GetComponent<CardDisplay>());
        //    return;
        //}
        if(isClickable)
        {
            ActivateCard();
        }
      
            
    }

    public void ActivateCard()
    {
        if (SpecialAbility == null)
        {
            Debug.Log("Discard");
            playerController.turnClick(false);
            Discard();
            playerController.InitHand();
            TurnSystem.Instance.EndTurn();
        }
        else
        {
            foreach (CardDisplay card in playerController.CardsInHand)
            {
                card.GetComponent<CardController>().isClickable = false;
            }
            Debug.Log("Click Ability");
            
            //Sc_GameLogic.Instance.SendMove(this.GetComponent<CardDisplay>());
            //Sc_GameLogic.Instance.Placement(this.GetComponent<CardDisplay>());
            InvokeAbility();
        }
    }

    public void AddAbility(Action abilityAction)
    {
        SpecialAbility += abilityAction;
        Debug.Log("SpecialAbility " + SpecialAbility);
    }

    public void InvokeAbility()
    {
        SpecialAbility?.Invoke();
    }
    public void drawNewCard()
    {
        playerController.InitHand();
    }
   
}
