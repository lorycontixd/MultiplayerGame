using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Lore.Stats;

public class Player : MonoBehaviourPunCallbacks
{
    public int MyView { get { return photonView.ViewID; } }

    public Stat MoveSpeed;

    void Start()
    {
        PunEventSender.Instance.SendPlayerSpawned(photonView.ViewID);
    }
}
