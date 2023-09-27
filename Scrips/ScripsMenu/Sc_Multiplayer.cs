using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Multiplayer : MonoBehaviour
{
    // API KEY 0745b440fe426ed8a4a6cd3820f04db9927eff22e8b562a5174f7ba2efb621d0
    // Secret Key  94dd68d914549d1fe9282292e814bf75777431ef3a4bddcb17cbb2791709a276
    private int value;
    public void Slider_numPlayer()
    {
        value =(int) GameObject.Find("Slider_PlayerNUM").GetComponent<Slider>().value;
        GameObject.Find("Txt_numPlayer").GetComponent<TextMeshProUGUI>().text = value.ToString()+"$";
    }
    #region AppWrap Keys
    //Do not work on ur project with my keys!
    private string apiKey = "0745b440fe426ed8a4a6cd3820f04db9927eff22e8b562a5174f7ba2efb621d0";
    private string secretKey = "94dd68d914549d1fe9282292e814bf75777431ef3a4bddcb17cbb2791709a276";

    private Listener listner;
    #endregion

    #region Variables

    public Dictionary<string, GameObject> unityObjects;
    private Dictionary<string, object> passedParams;
    private List<string> roomIds;
    private int roomIndex = 0;

    private int maxRoomUsers = 2;
    private string roomName = "ShenkarRoom";
    public string roomId;

    #endregion

    #region Singleton

    private static Sc_Multiplayer instance;
    public static Sc_Multiplayer Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("Sc_Multiplayer").GetComponent<Sc_Multiplayer>();

            return instance;
        }
    }

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnCreateRoom += OnCreateRoom;
        Listener.OnJoinRoom += OnJoinRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom += OnUserJoinRoom;
        Listener.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnJoinRoom -= OnJoinRoom;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom -= OnUserJoinRoom;
        Listener.OnGameStarted -= OnGameStarted;
    }

    void Awake()
    {
        InitAwake();
    }
    void Start()
    {
        InitStart();
    }

    #endregion

    #region Logic

    private void InitAwake()
    {
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _obj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach (GameObject g in _obj)
            unityObjects.Add(g.name, g);

        passedParams = new Dictionary<string, object>()
        {{"Password","Shenkar2023"}};

        listner = new Listener();
        WarpClient.initialize(apiKey, secretKey);
        WarpClient.GetInstance().AddConnectionRequestListener(listner);
        WarpClient.GetInstance().AddChatRequestListener(listner);
        WarpClient.GetInstance().AddUpdateRequestListener(listner);
        WarpClient.GetInstance().AddLobbyRequestListener(listner);
        WarpClient.GetInstance().AddNotificationListener(listner);
        WarpClient.GetInstance().AddRoomRequestListener(listner);
        WarpClient.GetInstance().AddTurnBasedRoomRequestListener(listner);
        WarpClient.GetInstance().AddZoneRequestListener(listner);

        GlobalVariables.userId = System.Guid.NewGuid().ToString(); ;
    }

    private void InitStart()
    {
        unityObjects["But_PlayMulty"].GetComponent<Button>().interactable = false;
        unityObjects["Screen_GameMultiplayer"].SetActive(false);

        //unityObjects["Txt_UserId"].GetComponent<TextMeshProUGUI>().text = "UserId: " + GlobalVariables.userId;

        WarpClient.GetInstance().Connect(GlobalVariables.userId);
        UpdateStatus("Open Connection...");
    }

    private void UpdateStatus(string _Str)
    {
        unityObjects["Txt_Status"].GetComponent<TextMeshProUGUI>().text = _Str;
    }

    private void DoRoomSearchLogic()
    {
        if (roomIndex < roomIds.Count)
        {
            UpdateStatus("Bring room info (" + roomIds[roomIndex] + ")");
            WarpClient.GetInstance().GetLiveRoomInfo(roomIds[roomIndex]);
        }
        else
        {
            UpdateStatus("Creating Room...");
            int _randNumber = UnityEngine.Random.Range(100000, 999999);
            WarpClient.GetInstance().CreateTurnRoom(roomName + _randNumber, GlobalVariables.userId, maxRoomUsers, passedParams, GlobalVariables.maxTurnTime);
        }
    }

    #endregion


    #region Server Callbacks

    private void OnConnect(bool _IsSuccess)
    {
        Debug.Log("OnConnect " + _IsSuccess);
        if (_IsSuccess)
            UpdateStatus("Connected.");
        else UpdateStatus("Failed to Connect.");

        unityObjects["But_PlayMulty"].GetComponent<Button>().interactable = _IsSuccess;
    }

    private void OnRoomsInRange(bool _IsSuccess, MatchedRoomsEvent eventObj)
    {
        Debug.Log("OnRoomsInRange " + _IsSuccess);
        if (_IsSuccess)
        {
            UpdateStatus("Parsing Rooms...");
            roomIds = new List<string>();
            foreach (var RoomData in eventObj.getRoomsData())
            {
                Debug.Log("Room Id: " + RoomData.getId());
                Debug.Log("Room Owner: " + RoomData.getRoomOwner());
                roomIds.Add(RoomData.getId());
            }

            roomIndex = 0;
            DoRoomSearchLogic();
        }
    }

    private void OnCreateRoom(bool _IsSuccess, string _RoomId)
    {
        Debug.Log("OnCreateRoom " + _IsSuccess + " " + _RoomId);
        if (_IsSuccess)
        {
            roomId = _RoomId;
            UpdateStatus("Room have been created, RoomId: " + _RoomId);
            WarpClient.GetInstance().JoinRoom(roomId);
            WarpClient.GetInstance().SubscribeRoom(roomId);
        }
        else
        {
            Debug.Log("Cant Create room...");
        }
    }

    private void OnJoinRoom(bool _IsSuccess, string _RoomId)
    {
        if (_IsSuccess)
        {
            UpdateStatus("Joined Room: " + _RoomId);
            //unityObjects["Txt_RoomId"].GetComponent<TextMeshProUGUI>().text = "Room Id: " + _RoomId;
        }
        else UpdateStatus("Failed to join Room: " + _RoomId);
    }

    private void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveRoomInfo ");
        if (eventObj != null && eventObj.getProperties() != null)
        {
            Dictionary<string, object> _properties = eventObj.getProperties();
            if (_properties.ContainsKey("Password") &&
                _properties["Password"].ToString() == passedParams["Password"].ToString())
            {
                roomId = eventObj.getData().getId();
                UpdateStatus("Received Room Info, joining room: " + roomId);
                WarpClient.GetInstance().JoinRoom(roomId);
                WarpClient.GetInstance().SubscribeRoom(roomId);
            }
            else
            {
                roomIndex++;
                DoRoomSearchLogic();
            }
        }
    }

    private void OnUserJoinRoom(RoomData eventObj, string _UserName)
    {
        UpdateStatus("User Joined Room " + _UserName);
        if (eventObj.getRoomOwner() == GlobalVariables.userId && GlobalVariables.userId != _UserName)
        {
            UpdateStatus("Starting Game...");
            WarpClient.GetInstance().startGame();
        }
    }

    private void OnGameStarted(string _Sender, string _RoomId, string _NextTurn)
    {
        UpdateStatus("Game Started, Next Turn: " + _NextTurn);
        unityObjects["Screen_Loading_Multiplayer"].SetActive(false);
        unityObjects["Img_Background"].SetActive(false);
        unityObjects["Screen_GameMultiplayer"].SetActive(true);
    }

    #endregion

    #region Controller

    public void Btn_PlayLogic()
    {
        Debug.Log("Btn_PlayLogic");
        unityObjects["But_PlayMulty"].GetComponent<Button>().interactable = false;
        WarpClient.GetInstance().GetRoomsInRange(1, 2);
        UpdateStatus("Searching for an available rooms...");
    }

    #endregion
}
