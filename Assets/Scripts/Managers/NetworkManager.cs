using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System.Linq;

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

    public bool playersConnected = false;
    public int spawnedCount;

    public List<Transform> spawnPoints;
    private static System.Random rng = new System.Random();

    public void MasterPickSpawnpoints()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            var numberList = Enumerable.Range(0, spawnPoints.Count - 1).ToList();
            var shuffledIndices = numberList.OrderBy(a => rng.Next()).ToList();
            foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in PhotonNetwork.CurrentRoom.Players)
            {
                PunEventSender.Instance.SendStartGame(kvp.Key, shuffledIndices[kvp.Key]);
            }
        }
    }

    public void StartGame(int spawnIndex)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name,spawnPoints[spawnIndex].position, Quaternion.identity);

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
            playersConnected = true;
            MasterPickSpawnpoints();
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
            playersConnected = true;
            MasterPickSpawnpoints();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
    }

    #endregion

}
