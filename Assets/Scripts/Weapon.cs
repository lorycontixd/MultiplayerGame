using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Weapon : MonoBehaviourPunCallbacks
{
    public Player player;
    public PlayerMovement pMovement;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
        if (other.tag == "Player")
        {
            Player otherPlayer = other.GetComponent<Player>();
            if (otherPlayer.photonView.ViewID != player.photonView.ViewID)
            {

                PunEventSender.Instance.SendDamage(otherPlayer.photonView.ViewID, player.Damage.Value);
            }
        }
    }
}
