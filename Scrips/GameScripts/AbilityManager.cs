using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public CardController cardController;
    private void Start()
    {
        cardController = GetComponent<CardController>();
        cardController.AddAbility(Ability);
    }

    public virtual void Ability()
    {

    }
 
    public void EndTurn()
    {
        GetComponent<CardController>().drawNewCard();
        TurnSystem.Instance.EndTurn();
    }
}
