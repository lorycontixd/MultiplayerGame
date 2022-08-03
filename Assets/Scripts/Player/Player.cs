using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{
    public int MyView { get { return photonView.ViewID; } }

    void Start()
    {
        PunEventSender.Instance.SendPlayerSpawned(photonView.ViewID);
    }
}
