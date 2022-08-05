using Lore.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInteractor : MonoBehaviourPunCallbacks
{
    Player player;


    private void Start()
    {
        if (photonView.IsMine)
        {
            player = GetComponent<Player>();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.collider.tag == "Player")
            {
                Player otherPlayer = collision.collider.gameObject.GetComponent<Player>();
                Vector3 force = collision.relativeVelocity;
                PunEventSender.Instance.SendForce(otherPlayer.photonView.ViewID, force);
                Debug.Log($"adding impact of {force}");
            }
            if (collision.collider.tag == "PowerUp")
            {
                PowerUp powerup = collision.gameObject.GetComponent<PowerUp>();
                PunEventSender.Instance.SendPowerUpPickUp(photonView.ViewID, powerup.number);
            }
        }
    }

    public IEnumerator OnPowerupPickup(PowerUp powerup)
    {
        if (photonView.IsMine)
        {
            switch (powerup.statType)
            {
                case StatType.MoveSpeed:
                    player.MoveSpeed.AddModifier(powerup.modifier);
                    Debug.Log("[+] New movespeed: " + player.MoveSpeed.Value);
                    yield return new WaitForSeconds(powerup.duration);
                    player.MoveSpeed.RemoveModifier(powerup.modifier);
                    Debug.Log("[-] New movespeed: " + player.MoveSpeed.Value);
                    break;
                case StatType.Damage:
                    player.Damage.AddModifier(powerup.modifier);
                    Debug.Log("New damage: " + player.Damage.Value);
                    yield return new WaitForSeconds(powerup.duration);
                    player.Damage.RemoveModifier(powerup.modifier);
                    break;
            }
        }
    }

    private void Update()
    {
    }
}
