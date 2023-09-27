using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    public static DiscardManager Instance;

    public Transform DiscardPoint;
    public List<CardDisplay> Discards = new List<CardDisplay>();
    public Deck _deck;
   

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
   

    public void MoveCardToDiscardList(CardDisplay card)
    {
        Discards.Add(card);
        card.transform.SetParent(this.transform);
        card.transform.localPosition = transform.localPosition;
        card.transform.rotation = transform.rotation;
    }
   
    public void Movecardtodeck()
    {
        _deck.isDoneShuffle = false;

        for (int i=0;i<Discards.Count;i++)
        {
            CardDisplay card = Discards[i];
                _deck.addCard(card);
        }
        _deck.shuffle();
        Discards.Clear();
    }
}
