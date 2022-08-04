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

    public Player GetPlayer(int viewID)
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (player.MyView == viewID)
            {
                return player;
            }
        }
        return null;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        
        switch (eventCode)
        {
            case PunEventSender.ErrorCode:
                {

                }
                break;
            case PunEventSender.NotificationCode:
                {

                }
                break;
            case PunEventSender.StartGameCode:
                {

                }
                break;
            case PunEventSender.PlayerSpawnedCode:
                {

                }
                break;
            case PunEventSender.ForceCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int playerID = (int)data[0];
                    Vector3 dir = (Vector3)data[1];
                    float force = (float)data[2];
                    Player player = GetPlayer(playerID);
                    if (player != null)
                    {
                        Debug.Log($"I received a force of intensity {force}");
                        Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
                        rb.AddForce(force * dir, ForceMode.Impulse);
                    }
                }
                break;
            case PunEventSender.DamageCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int playerID = (int)data[0];
                    float damage = (float)data[1];
                    Player p = GetPlayer(playerID);
                    p.TakeDamage(damage);
                }
                break;
            case PunEventSender.SpawnPowerupCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int powerupIndex = (int)data[0];
                    Vector3 pos = (Vector3)data[1];
                    GameManager.Instance.SpawnPowerUp(powerupIndex, pos);
                }
                break;
            case PunEventSender.PickupPowerupCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int powerupIndex = (int)data[0];
                    PowerUp[] powerups = FindObjectsOfType<PowerUp>();
                    foreach (PowerUp powerup in powerups)
                    {
                        if (powerup.number == powerupIndex)
                        {
                            Destroy(powerup.gameObject);
                            break;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }
}
