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

    public Stat Damage;
    public Stat MoveSpeed;
    public float MaxHealth;
    private float health;
    public float Mass;

    void Start()
    {
        PunEventSender.Instance.SendPlayerSpawned(photonView.ViewID);
        health = MaxHealth;
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
    #endregion
}
