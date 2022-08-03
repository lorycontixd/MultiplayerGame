using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static NetworkManager _instance;
    public static NetworkManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public int maxRoomNumber = 9999;

    public CinemachineVirtualCamera virtualCamera;
    public GameObject playerPrefab;
    public GameObject mainCamera;
    public Transform centerSpawner;

    public void StartGame()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3( centerSpawner.position.x + Random.Range(-10f, 10f), centerSpawner.position.y + Random.Range(-10f, 10f), centerSpawner.position.z), Quaternion.identity);
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;

        mainCamera.SetActive(false);
    }

    #region Pun Functions
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetNickname(string nickname)
    {
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }
    #endregion

    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            StartGame();
        }
    }

    public override void OnCreatedRoom()
    {
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int number = UnityEngine.Random.Range(0, maxRoomNumber);
        string roomName = $"Room{number}";
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers=2, IsOpen = true, IsVisible=true}, typedLobby: TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            StartGame();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
    }

    #endregion

}
