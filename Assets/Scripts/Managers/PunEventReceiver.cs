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
            if (player.photonView.ViewID == viewID)
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
                    object[] data = (object[])photonEvent.CustomData;
                    int playerID = (int)data[0];
                    int spawnIndex = (int)data[1];
                    Debug.Log(spawnIndex + " - " + PhotonNetwork.LocalPlayer.ActorNumber);
                    if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        NetworkManager.Instance.StartGame(spawnIndex);
                    }
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
                    Vector3 force = (Vector3)data[1];
                    GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject go in playersGO)
                    {
                        Player player = go.GetComponent<Player>();
                        if (player.photonView.ViewID == playerID)
                        {
                            go.GetComponent<PlayerMovement>().AddForce(force);
                            break;
                        }
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
                        if (player.photonView.ViewID == playerID)
                        {
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
                    int playerID = (int)data[0];
                    int powerupIndex = (int)data[1];
                    GameObject player = null;
                    GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject go in playersGO)
                    {
                        Player p = go.GetComponent<Player>();
                        if (p.photonView.ViewID == playerID)
                        {
                            player = go;
                            break;
                        }
                    }
                    if (player == null)
                    {
                        throw new UnityException($"Did not find a player with viewID {playerID} to give buff to.");
                    }
                    Debug.Log($"Giving buff to player with view {player.GetPhotonView()}");
                    PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

                    PowerUp[] powerups = FindObjectsOfType<PowerUp>();
                    foreach (PowerUp powerup in powerups)
                    {
                        if (powerup.number == powerupIndex)
                        {
                            StartCoroutine(interactor.OnPowerupPickup(powerup));
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
