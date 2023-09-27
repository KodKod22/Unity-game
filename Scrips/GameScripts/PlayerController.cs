using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Deck deck;
    public Transform[] handPositions;
    public List<CardDisplay> CardsInHand = new List<CardDisplay>();

    [SerializeField]
    public TextMeshProUGUI queensCount_txt;
    public int queensCount = 0;

    public int totalCardsInHand = 5;
    public List<QueenCard> TotalQueens = new List<QueenCard>();
    public CardController LastPlayedCard;

    private void Awake()
    {
        deck = FindObjectOfType<Deck>();
    }
    public void Start()
    {
        StartCoroutine(WaitForDeckToComplete());
        queensCount_txt.text = queensCount.ToString();
    }
   
    private IEnumerator WaitForDeckToComplete()
    {
        yield return new WaitUntil(() => deck.CardsInDeck.Count == deck.TotalDeckCards);
        InitHand();

        yield break;
    }
    public void InitHand()
    {
        for (int i = 0; i < handPositions.Length; i++)
        {
            if (handPositions[i].childCount == 0)
            {
                CardDisplay newCard = deck.DrawCard();
                
                if (newCard == null)
                {
                    break;
                }
                CardController controller = newCard.GetComponent<CardController>();
                RectTransform rect = newCard.GetComponent<RectTransform>();
                rect.transform.SetParent(handPositions[i]);
                rect.anchoredPosition = Vector2.zero;
                rect.rotation = Quaternion.Euler(0, 0, 0);
                CardsInHand.Add(newCard);
                if(GetComponent<AIPlayer>())
                {
                    newCard.CardCover(true);
                }
                else
                {
                    newCard.CardCover(false);
                }
                controller.playerController = this;
                if (TurnSystem.Instance.Isyourturn == false)
                {
                    Debug.Log("" + newCard.name);
                }

            }
            else
            {
                continue;
            }
        }

        turnClick(true);

        //if (Sc_GameLogic.Instance.Isyourturn == false)
        //{
        //    BattleField.Instance.CoverPlayerHand();
        //}

    }
    public void turnClick(bool turn)
    {
        
        for (int i=0;i<CardsInHand.Count;i++)
        {
           CardsInHand[i].GetComponent<CardController>().isClickable = turn;
            
        }

    }
    public void AddQueen(QueenCard queen)
    {
        Debug.Log("###### adding queen!");
        queensCount++;
        TotalQueens.Add(queen);
        queensCount_txt.text = queensCount.ToString();

    }
    public void RemoveQuenn(QueenCard queen)
    {
        Debug.Log("@@@@@@@@ removing queen!");

        queensCount--;
        TotalQueens.Remove(queen);
        queensCount_txt.text = queensCount.ToString();

    }
    public void ResetHand()
    {
        // Clear the list of cards in the player's hand
        foreach (Transform card in handPositions)
        {
            Destroy(card.gameObject);
        }
        CardsInHand.Clear();

        // Initialize the player's hand again
        InitHand();
    }
}
