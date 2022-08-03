using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunEventSender : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static PunEventSender _instance;
    public static PunEventSender Instance { get { return _instance; } }

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

    // Generic
    public const byte ErrorCode = 0;
    public const byte NotificationCode = 1;

    // Pre game
    public const byte StartGameCode = 2;
    public const byte PlayerSpawnedCode = 3;

    // In game
    public const byte ForceCode = 4;

    #region Event Functions
    public void SendError(string errorMsg)
    {
        object[] content = new object[] {errorMsg} ; 
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(ErrorCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendNotification(string notification)
    {
        object[] content = new object[] { notification };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(NotificationCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendStartGame()
    {
        object[] content = new object[] { };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(StartGameCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPlayerSpawned(int playerID)
    {
        object[] content = new object[] { playerID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PlayerSpawnedCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendForce(int targetPlayer, Vector3 dir, float force)
    {
        object[] content = new object[] { targetPlayer, dir, force};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PlayerSpawnedCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    #endregion
}
