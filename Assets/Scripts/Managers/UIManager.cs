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

    [Header("Starting components")]
    public InputField usernameField;
    public Button connectButton;
    public Text connectingText;

    [Header("Debug components")]
    public TextMeshProUGUI filler;

    [Header("Waiting for players components")]
    public TextMeshProUGUI waitingForPlayersText;

    [Header("Prefabs")]
    public GameObject debugTextPrefab;

    [Header("Variables")]
    public float debugMessageDuration = 2f;

    private void Start()
    {
        startingPanel.SetActive(true);
        debugPanel.SetActive(true);
        waitingForPlayersPanel.SetActive(false);
        connectingText.gameObject.SetActive(false);
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

    public void Debug(string text)
    {
        StartCoroutine(DebugCoroutine(text));
    }

    public IEnumerator DebugCoroutine(string text)
    {
        debugPanel.gameObject.SetActive(true);
        GameObject clone = Instantiate(debugTextPrefab, debugPanel.transform);
        clone.GetComponent<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(debugMessageDuration);
        Destroy(clone.gameObject);
        if (debugPanel.transform.childCount <= 0)
        {
            debugPanel.SetActive(false);
        }
    }

    #region Pun Callbacks
    public override void OnJoinedRoom()
    {
        Debug($"Joind room {PhotonNetwork.CurrentRoom.Name}");
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
        Debug($"Created room {PhotonNetwork.CurrentRoom.Name}");
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
