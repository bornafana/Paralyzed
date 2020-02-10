using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject playButton;
    public GameObject cancelButton;

    void Awake()
    {
        lobby = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton.SetActive(true);
    }

    public void OnPlayButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
        playButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        playButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join random game but failed. No open rooms available");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create new room. Change room name");
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 100);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers};
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOptions);
        Debug.Log("Creating a new room");
    }
}

