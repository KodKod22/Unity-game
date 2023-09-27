using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Deck : MonoBehaviour
{
    public List<CardDisplay> OptionalCards = new();
    public List<CardDisplay> CardsInDeck = new();
    public List<CardDisplay> continer = new();
    public int TotalDeckCards;
    public float offsetY = 0;

    public bool isDoneShuffle = false;
    void Awake()
    {
        InitDeck();
        shuffle();
    }

    public void InitDeck()
    {
        for (int i = 0; i < OptionalCards.Count; i++)
        {
            CardDisplay newCardDisplay = OptionalCards[i];
            AddCardToList(newCardDisplay);
        }
    }
   

   public void AddCardToList(CardDisplay cardDisplay)
    {
        for (int i = 0; i < cardDisplay.card.duplicateCount; i++)
        {
            cardDisplay.GetComponent<CardController>().isClickable = false;
            GameObject newCard = Instantiate(cardDisplay.gameObject, transform);

            offsetY += 0.05f;
            newCard.transform.position = new Vector3(newCard.transform.position.x, newCard.transform.position.y + offsetY, newCard.transform.position.z);
            CardsInDeck.Add(newCard.GetComponent<CardDisplay>());
        }
    }
    public void addCard(CardDisplay d_card)
    {
        CardsInDeck.Add(d_card);
        d_card.transform.SetParent(this.transform);
        d_card.transform.localPosition = transform.localPosition;
        d_card.transform.rotation = transform.rotation;
        d_card.CardCover(true);
    }

    public CardDisplay DrawCard()
    {
        if (CardsInDeck.Count > 0)
        {
            CardDisplay newCard = CardsInDeck[UnityEngine.Random.Range(0, CardsInDeck.Count)];

            CardsInDeck.Remove(newCard);
            return newCard;
        }
        else
        {
            return null;
        }
      
    }
   
        public void shuffle()
    {
        for (int i=0;i<CardsInDeck.Count;i++)
        {
            continer[0] = CardsInDeck[i];
            int randomIndex = Random.Range(0,CardsInDeck.Count);
            CardsInDeck[i] = CardsInDeck[randomIndex];
            CardsInDeck[randomIndex] = continer[0];
        }

        isDoneShuffle = true;
    }
}
