using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleField : MonoBehaviour
{
    public static BattleField Instance;

    public List<CardDisplay> OptionalQueens = new();
    public List<CardDisplay> CardsInDeckQueens = new();
    public List<CardDisplay> CardsInSlot = new List<CardDisplay>();
    public Transform[] SlotPositions;
    public int totlQueenslot = 12;
    private int WinnerScour = 4;//4
    [Header("Players")]
    public PlayerController P1;
    public AIPlayer P2;
    public Image winnerImg;
    public bool isGameOver = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        IntQueenAwake();
        QueenScater();
        
    }
    private void Start()
    {
        
        winnerImg.gameObject.SetActive(false);
        //winnerImg.GetComponentInChildren<Button>().gameObject.SetActive(false);
    }
    private void Update()
    {
        ChackForWiner();
    }
    public void IntQueenAwake()
    {
        for (int i = 0; i < OptionalQueens.Count; i++)
        {
            CardDisplay newcard = OptionalQueens[i];
            AddCardToDeckQueens(newcard);
        }
    }


    private void AddCardToDeckQueens(CardDisplay cardDisplay)
    {
        for (int i = 0; i < cardDisplay.card.duplicateCount; i++)
        {

            GameObject newCard = Instantiate(cardDisplay.gameObject, transform);
            CardsInDeckQueens.Add(newCard.GetComponent<CardDisplay>());
        }
    }

    public void QueenScater()
    {
        for (int i = 0; i < totlQueenslot; i++)
        {
            CardDisplay newCard = DrawQueenCard();
            CardController controller = newCard.GetComponent<CardController>();
            RectTransform rect = newCard.GetComponent<RectTransform>();
            rect.transform.SetParent(SlotPositions[i]);
            rect.anchoredPosition = new Vector2(0,0);
            rect.rotation = Quaternion.Euler(0, 0, 0);
            CardsInSlot.Add(newCard);
            newCard.CardCover(true);
        }
    }
    
    public CardDisplay DrawQueenCard()
    {
        CardDisplay newCard = CardsInDeckQueens[UnityEngine.Random.Range(0, CardsInDeckQueens.Count)];
        CardsInDeckQueens.Remove(newCard);

        return newCard;
    }
    

    
    public void placeQueens(CardDisplay queen)
    {
        for (int i = 0; i < totlQueenslot; i++)
        {
            if (SlotPositions[i].childCount > 0 && SlotPositions[i].GetChild(0).name == queen.name)
            {
                Debug.Log("im here");
                queen.gameObject.SetActive(true);
               
            }
            else
            {
                continue;
            }
        }

    }
    public void ChackForWiner()
    {
        if (P1.TotalQueens.Count == WinnerScour)
        {
            isGameOver = true;
            Debug.Log("p1 is the winner");
            winnerImg.gameObject.SetActive(true);
           
            winnerImg.GetComponentInChildren<TextMeshProUGUI>().text = "you are the winner";
            
        }
        else if (P2.TotalQueens.Count == WinnerScour)
        {
            isGameOver = true;
            Debug.Log("p2 is the winner");
            
            winnerImg.gameObject.SetActive(true);
            
            winnerImg.GetComponentInChildren<TextMeshProUGUI>().text = "computer is the winner";
            
        }
        
    }
    public void ResetGame()
    {
        isGameOver = false;   

        // Reset player 1
        P1.TotalQueens.Clear();
        P1.queensCount = 0;
        P1.queensCount_txt.text = P1.queensCount.ToString();
        foreach (CardDisplay c in P1.CardsInHand)
        {
            Destroy(c.gameObject);
        }
        P1.CardsInHand.Clear();

        P2.TotalQueens.Clear();
        P2.queensCount = 0;
        P2.queensCount_txt.text = P2.queensCount.ToString();
        foreach (CardDisplay c in P2.CardsInHand)
        {
            Destroy(c.gameObject);
        }
        P2.CardsInHand.Clear();
        // Set the turn system and winner image
        TurnSystem.Instance.UpdateTurn();
        winnerImg.gameObject.SetActive(false);

        
        foreach (CardDisplay c in CardsInSlot)
        {
            c.gameObject.SetActive(true);
        }

        TurnSystem.Instance.Isyourturn = true;


        DiscardManager.Instance.Movecardtodeck();
        Invoke("RestartGame", 2f);
    }

    private void RestartGame()
    {
        P1.InitHand();
        P2.InitHand();
    }

}
