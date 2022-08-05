using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Lore.Stats;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public Stat Damage;
    public Stat MoveSpeed;
    public float MaxHealth;
    public float health;
    public float Mass;

    void Start()
    {
        if (photonView.IsMine)
        {
            Player.LocalPlayerInstance = this.gameObject;
            PunEventSender.Instance.SendPlayerSpawned(photonView.ViewID);
            health = MaxHealth;
        }
        DontDestroyOnLoad(this.gameObject);
        
    }

    #region Health
    public void TakeDamage(float damage)
    {
        damage = Mathf.Abs(damage);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void HealDamage(float damage)
    {
        damage = Mathf.Abs(damage);
        health += damage;
    }

    public void Die()
    {
        Debug.Log("Dead!!");
        UIManager.Instance.deathPanel.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            this.health = (float)stream.ReceiveNext();
        }
    }
    #endregion
}
