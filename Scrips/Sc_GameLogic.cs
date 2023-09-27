using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sc_GameLogic : MonoBehaviour
{
    public static Sc_GameLogic Instance;
    private string nextTurn;
    private float startTime;
    public TextMeshProUGUI turnText;

    private void OnEnable()
    {
        //SC_Slot.OnClickSlot += OnClickSlot;
        Listener.OnGameStarted += OnGameStarted;
        Listener.OnMoveCompleted += OnMoveCompleted;
        Listener.OnGameStopped += OnGameStopped;
        Listener.OnSendChat += OnSendChat;
    }

    private void OnDisable()
    {
        //SC_Slot.OnClickSlot -= OnClickSlot;
        Listener.OnGameStarted -= OnGameStarted;
        Listener.OnMoveCompleted -= OnMoveCompleted;
        Listener.OnGameStopped -= OnGameStopped;
        Listener.OnSendChat -= OnSendChat;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   

   
    public void SendMove(CardDisplay card)
    {
        Dictionary<string, object> _toSend = new Dictionary<string, object>() { { "Card", card } };
        string _toJson = MiniJSON.Json.Serialize(_toSend);
        Debug.Log(_toJson);
        WarpClient.GetInstance().sendMove(_toJson);
    }
    public void Placement(CardDisplay card)
    {
        if (card!= null)
        {
            card.GetComponent<CardController>().ActivateCard();
        }
    }

    private void OnGameStarted(string _Sender, string _RoomId, string _NextTurn)
    {
        BattleField.Instance.isGameOver = false;
        Debug.Log("_Sender " + _Sender + ", _RoomId: " + _RoomId + ", _NextTurn: " + _NextTurn);
        nextTurn = _NextTurn;
        startTime = Time.time;

        //My turn
        if (GlobalVariables.userId == _NextTurn)
        {
           TurnSystem.Instance.Isyourturn= true;
            foreach (CardDisplay card in BattleField.Instance.P1.CardsInHand)
            {
                card.GetComponent<CardController>().isClickable = true;
            }
            TurnSystem.Instance.UpdateTurn();
        }
        else //Opponent turn
        {
            TurnSystem.Instance.Isyourturn = false;
            foreach (CardDisplay card in BattleField.Instance.P1.CardsInHand)
            {
                card.GetComponent<CardController>().isClickable = false;
            }
            TurnSystem.Instance.UpdateTurn();
        }
    }

    private void OnMoveCompleted(MoveEvent _Move)
    {
        Debug.Log("OnMoveCompleted " + _Move.getNextTurn() + " " + _Move.getMoveData() + " " + _Move.getSender());
        if (_Move.getSender() != GlobalVariables.userId && _Move.getMoveData() != null)
        {
            string _senderJson = _Move.getMoveData();
            Dictionary<string, object> _data = (Dictionary<string, object>)MiniJSON.Json.Deserialize(_senderJson);
            if (_data.ContainsKey("Card"))
            {
                CardDisplay Card = (CardDisplay)_data["Card"];
                 
                Placement(Card);
            }
        }
        else if (_Move.getMoveData() == null)
        {
            TurnSystem.Instance.EndTurn();
        }
                    

        startTime = Time.time;
        if (_Move.getNextTurn() == GlobalVariables.userId)
        {
            TurnSystem.Instance.Isyourturn = true;

        }

        else
        {
            TurnSystem.Instance.Isyourturn = false;
        }

        if (BattleField.Instance.isGameOver)
        {
            WarpClient.GetInstance().stopGame();
        }
            
    }

    private void OnGameStopped(string _Sender, string _RoomId)
    {
        Debug.Log("Game is over, " + _Sender + " " + _RoomId);
    }

    private void OnSendChat(string _Sender, string _Message)
    {
        Debug.Log("<color=blue> " + _Sender + " " + _Message + "</color>");
    }
}
