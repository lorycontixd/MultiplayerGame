using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    private bool isMasterClient;
    public float GameTime;

    /////////// Game Settings
    // Power ups
    [Header("Power ups")]
    
    [Tooltip("How long the powerup lasts on the ground")]
    public float powerUpDuration = 7f;
    [Tooltip("After how many seconds a new powerup spawns on the map")]
    public float powerUpSpawnCooldown = 10f;
    [Tooltip("How many powerups can stay on the ground at once")]
    public int maxPowerUpCount = 4;
    [Tooltip("After how many seconds the first power up spawns")]
    public float powerUpFirstSpawnTimer = 3f;
    [Tooltip("The transform that holds all powerups")]
    public Transform powerUpHolder;
    [Tooltip("List of powerup prefabs to spawn")]
    public List<GameObject> powerUps;

    public float powerUpSpawnXMIN = 3f;
    public float powerUpSpawnZMIN = -3f;
    public float powerUpSpawnXMAX = 97f;
    public float powerUpSpawnZMAX = -97f;
    public float powerUpSpawnY = 1.1f;

    private int powerUpCount;
    private float powerUpTimer;

    //

    private void Start()
    {
        powerUpCount = 0;
        powerUpTimer = powerUpFirstSpawnTimer;
    }

    private void Update()
    {
        if (NetworkManager.Instance.playersConnected)
        {
            GameTime += Time.deltaTime;
            Debug.Log("=> is master client: " + PhotonNetwork.LocalPlayer.IsMasterClient);
            if (powerUpTimer <= GameTime)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("Masterclient sending event to spawn a powerup");
                    SendSpawnPowerUp();
                    powerUpTimer = GameTime + powerUpSpawnCooldown;
                }
            }
        }
    }

    #region MasterClient Functions

    public void SendSpawnPowerUp()
    {
        if (powerUpCount < maxPowerUpCount)
        {
            int randomPowerUp = Random.Range(0, powerUps.Count - 1);
            Debug.Log($"random number powerup: {randomPowerUp}");
            Vector3 pos = new Vector3(
                Random.Range(powerUpSpawnXMIN, powerUpSpawnXMAX),
                powerUpSpawnY,
                Random.Range(powerUpSpawnZMIN, powerUpSpawnZMAX)
            );
            PunEventSender.Instance.SendPowerupSpawn(randomPowerUp, pos);
        }
    }

    #endregion

    #region General Functions
    // Power Ups
    public void SpawnPowerUp(int index, Vector3 pos)
    {
        StartCoroutine(SpawnPowerUpCoroutine(index, pos));
    }

    public IEnumerator SpawnPowerUpCoroutine(int index, Vector3 pos)
    {
        GameObject clone = PhotonNetwork.Instantiate(powerUps[index].name, pos, Quaternion.identity);
        PowerUp powerup = clone.GetComponent<PowerUp>();
        powerup.number = powerUpCount;
        clone.transform.SetParent(powerUpHolder.transform);
        powerUpCount++;
        yield return new WaitForSeconds(powerUpDuration);
        Destroy(clone);
        powerUpCount--;
    }

    //
    #endregion
}
