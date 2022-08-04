using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class UIManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

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

    [Header("Panels")]
    public GameObject startingPanel;
    public GameObject debugPanel;
    public GameObject waitingForPlayersPanel;
    public GameObject playersTabPanel;
    public GameObject deathPanel;

    [Header("Starting components")]
    public InputField usernameField;
    public Button connectButton;
    public Text connectingText;

    [Header("Debug components")]
    public TextMeshProUGUI filler;

    [Header("Waiting for players components")]
    public TextMeshProUGUI waitingForPlayersText;

    [Header("Players Tab Components")]
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI player1PingText;
    public TextMeshProUGUI player2PingText;
    public TextMeshProUGUI player1Host;
    public TextMeshProUGUI player2Host;
    public TextMeshProUGUI roomNameText;
    public int player1Ping;
    public int player2Ping;

    public int greenPingThreshold = 60;
    public int yellowPingThreshold = 120;

    public float pingCooldown = 5f;
    private float pingTimer;

    [Header("Prefabs")]
    public GameObject debugTextPrefab;

    [Header("Variables")]
    public float debugMessageDuration = 2f;

    private void Start()
    {
        startingPanel.SetActive(true);
        debugPanel.SetActive(true);
        waitingForPlayersPanel.SetActive(false);
        playersTabPanel.SetActive(false);
        deathPanel.SetActive(false);
        connectingText.gameObject.SetActive(false);
    }


    private void Update()
    {
        Debug.Log("PING: "+PhotonNetwork.GetPing());
        if (NetworkManager.Instance.playersConnected)
        {
            // Players tab
            if (Input.GetKey(KeyCode.Tab))
            {
                playersTabPanel.SetActive(true);
            }
            else
            {
                playersTabPanel.SetActive(false);
            }
            UpdatePlayersTabPings();
        }
    }


    public IEnumerator SetupPlayersTab()
    {
        yield return new WaitUntil(() => NetworkManager.Instance.playersConnected);
        player1Text.text = PhotonNetwork.CurrentRoom.Players[0].NickName;
        player2Text.text = PhotonNetwork.CurrentRoom.Players[1].NickName;
        player1Host.text = PhotonNetwork.CurrentRoom.Players[0].IsMasterClient ? "Host" : "";
        player2Host.text = PhotonNetwork.CurrentRoom.Players[1].IsMasterClient ? "Host" : "";
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void UpdatePlayersTabPings()
    {
        if (NetworkManager.Instance.playersConnected && playersTabPanel.activeSelf)
        {
            if (pingTimer <= GameManager.Instance.GameTime)
            {
                PunEventSender.Instance.SendPing(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.GetPing());
                player1PingText.text = player1Ping.ToString();
                if (player1Ping < greenPingThreshold)
                {
                    player1PingText.color = Color.green;
                }else if (player1Ping >= greenPingThreshold && player1Ping < yellowPingThreshold)
                {
                    player1PingText.color = Color.yellow;
                }
                else
                {
                    player1PingText.color = Color.red;
                }
                player2PingText.text = player2Ping.ToString();
                if (player2Ping < greenPingThreshold)
                {
                    player2PingText.color = Color.green;
                }
                else if (player2Ping >= greenPingThreshold && player2Ping < yellowPingThreshold)
                {
                    player2PingText.color = Color.yellow;
                }
                else
                {
                    player2PingText.color = Color.red;
                }
                pingTimer = GameManager.Instance.GameTime + pingCooldown;
            }
        }
    }

    public void OnConnectButton()
    {
        if (usernameField.text != string.Empty)
        {
            connectButton.gameObject.SetActive(false);
            usernameField.gameObject.SetActive(false);
            connectingText.gameObject.SetActive(true);

            NetworkManager.Instance.Connect();
            NetworkManager.Instance.SetNickname(usernameField.text);
        }
    }

    public void DisableStartingUI()
    {
        startingPanel.SetActive(false);
    }

    public void DebugMessage(string text)
    {
        StartCoroutine(DebugMessageCoroutine(text));

    }

    public IEnumerator DebugMessageCoroutine(string text)
    {
        debugPanel.gameObject.SetActive(true);
        GameObject clone = Instantiate(debugTextPrefab, debugPanel.transform);
        clone.GetComponent<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(debugMessageDuration);
        Destroy(clone.gameObject);
        yield return new WaitForSeconds(0.05f);
        Debug.Log($"childcount: {debugPanel.transform.childCount}");
        if (debugPanel.transform.childCount <= 0)
        {
            debugPanel.SetActive(false);
        }
    }

    #region Pun Callbacks
    public override void OnJoinedRoom()
    {
        DebugMessage($"Joined room {PhotonNetwork.CurrentRoom.Name}");
        DisableStartingUI();
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            waitingForPlayersPanel.SetActive(true);
        }
        else
        {
            waitingForPlayersPanel.SetActive(false);
        }
    }

    public override void OnCreatedRoom()
    {
        DebugMessage($"Created room {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            waitingForPlayersPanel.SetActive(false);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }
    }
    #endregion
}
