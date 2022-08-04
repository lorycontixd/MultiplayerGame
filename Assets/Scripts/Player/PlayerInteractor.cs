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
        player = GetComponent<Player>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (collision.collider.tag == "PowerUp")
        {
            PowerUp powerup = collision.gameObject.GetComponent<PowerUp>();
            powerup.OnPickUp();
            PunEventSender.Instance.SendPowerUpPickUp(powerup.number);
            StartCoroutine(OnPowerupPickup(powerup.duration,powerup.statType, powerup.modifier));
        }
    }

    private IEnumerator OnPowerupPickup(float duration, StatType statType, StatModifier mod)
    {
        switch (statType)
        {
            case StatType.MoveSpeed:
                player.MoveSpeed.AddModifier(mod);
                Debug.Log("[+] New movespeed: " + player.MoveSpeed.Value);
                yield return new WaitForSeconds(duration);
                player.MoveSpeed.RemoveModifier(mod);
                Debug.Log("[-] New movespeed: " + player.MoveSpeed.Value);
                break;
            case StatType.Damage:
                player.Damage.AddModifier(mod);
                Debug.Log("New damage: " + player.Damage.Value);
                yield return new WaitForSeconds(duration);
                player.Damage.RemoveModifier(mod);
                break;
        }
    }
}
