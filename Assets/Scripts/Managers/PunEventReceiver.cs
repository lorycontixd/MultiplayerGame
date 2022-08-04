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
                        rb.AddForce(force * dir, ForceMode.VelocityChange);
                    }
                }
                break;
            case PunEventSender.DamageCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int playerID = (int)data[0];
                    float damage = (float)data[1];
                    GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject go in playersGO)
                    {
                        Player player = go.GetComponent<Player>();
                        Debug.Log($"Searching player for damage: {playerID}/{player.photonView.ViewID}");
                        if (player.photonView.ViewID == playerID)
                        {
                            Debug.Log("Found!!");
                            player.TakeDamage(damage);
                        }
                    }
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
                            Debug.Log($"Destroying buff with number {powerup.number}");
                            Destroy(powerup.gameObject);
                            break;
                        }
                    }
                }
                break;
            case PunEventSender.PingCode:
                {
                    object[] data = (object[])photonEvent.CustomData;
                    int playerID = (int)data[0];
                    int ping = (int)data[1];
                    if (playerID == 0)
                    {
                        UIManager.Instance.player1Ping = ping;
                    }else if (playerID == 1)
                    {
                        UIManager.Instance.player2Ping = ping;
                    }
                    else
                    {
                        PunEventSender.Instance.SendError($"Invalid playerID for ping communication_ {playerID}");
                    }
                }
                break;

            default:
                break;
        }
    }
}
