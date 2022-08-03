using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PunEventReceiver : MonoBehaviour, IOnEventCallback
{
    #region Singleton
    private static PunEventReceiver _instance;
    public static PunEventReceiver Instance { get { return _instance; } }

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

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        
        switch (eventCode)
        {
            case PunEventSender.ErrorCode:
                break;
            case PunEventSender.NotificationCode:
                break;
            case PunEventSender.StartGameCode:
                break;
            case PunEventSender.PlayerSpawnedCode:
                break;
            default:
                break;
        }
    }
}
