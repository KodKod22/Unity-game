using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;
    public bool Isyourturn;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI turnText1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Isyourturn = true;
        UpdateTurn();
    }

    public void EndTurn()
    {
        Isyourturn = !Isyourturn;
        UpdateTurn();
    }

    public void UpdateTurn()
    {
        if (Isyourturn)
        {
            turnText.text = "Your turn";
            turnText1.text = "Your turn";
            foreach (CardDisplay card in BattleField.Instance.P1.CardsInHand)
            {
                card.GetComponent<CardController>().isClickable = true;
            }
            //foreach (CardDisplay card in BattleField.Instance.P2.CardsInHand)
            //{
            //    card.GetComponent<CardController>().isClickable = false;
            //}
        }
        if (!Isyourturn)
        {
            turnText.text = "Opponet turn";
            turnText1.text = "Your turn";
            foreach (CardDisplay card in BattleField.Instance.P1.CardsInHand)
            {
                card.GetComponent<CardController>().isClickable = false;
            }
        }
        if (!Isyourturn && BattleField.Instance.P2.GetComponent<AIPlayer>())
        {
            StartCoroutine(TheMove());
        }
    }
    public IEnumerator TheMove()
    {
        yield return new WaitForSeconds(3.0f);
        BattleField.Instance.P2.GetComponent<AIPlayer>().MakeMove();
        yield break;
    }
}
