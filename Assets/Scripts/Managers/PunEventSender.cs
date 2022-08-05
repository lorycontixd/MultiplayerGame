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
    public const byte PingCode = 8;

    // In game
    public const byte ForceCode = 4;
    public const byte DamageCode = 5;
    public const byte SpawnPowerupCode = 6;
    public const byte PickupPowerupCode = 7;

    #region Event Functions
    public void SendError(string errorMsg)
    {
        object[] content = new object[] {errorMsg} ; 
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(ErrorCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendNotification(string notification)
    {
        object[] content = new object[] { notification };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(NotificationCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendStartGame(int playerID, int spawnIndex)
    {
        object[] content = new object[] { playerID, spawnIndex };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(StartGameCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPlayerSpawned(int playerID)
    {
        object[] content = new object[] { playerID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(PlayerSpawnedCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendForce(int targetPlayer, Vector3 dir, float force)
    {
        object[] content = new object[] { targetPlayer, dir, force};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(ForceCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendDamage(int targetPlayer, float damage)
    {
        Debug.Log($"[PunSender] Sending damage to {targetPlayer}");
        object[] content = new object[] { targetPlayer, damage };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(DamageCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPowerupSpawn(int powerUpIndex, Vector3 pos)
    {
        object[] content = new object[] { powerUpIndex, pos };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SpawnPowerupCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPowerUpPickUp(int powerupID)
    {
        object[] content = new object[] { powerupID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PickupPowerupCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPing(int playerID, int ping)
    {
        object[] content = new object[] { playerID, ping };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PingCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    #endregion
    }
