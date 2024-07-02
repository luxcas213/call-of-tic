using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class launcher : MonoBehaviourPunCallbacks
{
    public static launcher instance;

    void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_InputField RoomNameInputField;
    [SerializeField] TMP_Text errortext;
    [SerializeField] TMP_Text roomnametext;
    [SerializeField] Transform RoomListContent;
    [SerializeField] GameObject RoomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject StartGameButton;
    void Start()
    {
        Debug.Log("conecting to Master");

        PhotonNetwork.ConnectUsingSettings();
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to Master");

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public override void OnJoinedLobby()
    {
        Menumanager.Instance.OpenMenu("Title");
        Debug.Log("joined Lobby");
        
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(RoomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(RoomNameInputField.text);
        Menumanager.Instance.OpenMenu("Loading");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedRoom()
    {
        Menumanager.Instance.OpenMenu("Room");
        roomnametext.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<playerListItem>().SetUp(players[i]);

        }
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errortext.text = "Room Creation Failed" + message;
        Menumanager.Instance.OpenMenu("Error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Menumanager.Instance.OpenMenu("Loading");
    }
    public override void OnLeftRoom()
    {
        Menumanager.Instance.OpenMenu("Title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in RoomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(RoomListItemPrefab, RoomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);

        }   
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        Menumanager.Instance.OpenMenu("Loading");

        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<playerListItem>().SetUp(newPlayer);
    }
}
