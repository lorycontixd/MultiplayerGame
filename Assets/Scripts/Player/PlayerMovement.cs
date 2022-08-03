using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Lore.Stats;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    Player player;
    Rigidbody m_Rigidbody;

    private float playerSpeed;
    private bool canMove;
    public float pushForce = 3f;

    void Start()
    {
        //Fetch player component where all stats lie
        player = GetComponent<Player>();
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        canMove = photonView.IsMine;
        playerSpeed = player.MoveSpeed.Value;

        player.MoveSpeed.onValueChange += OnMoveSpeedChange;
    }

    private void OnMoveSpeedChange(Stat obj)
    {
        playerSpeed = obj.Value;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colliding with {collision.collider.name}");
        if ( collision.collider.tag == "Player")
        {
            PunEventSender.Instance.SendForce(collision.gameObject.GetComponent<Player>().MyView, collision.transform.position - transform.position, pushForce);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            //Store user input as a movement vector
            Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //Apply the movement vector to the current position, which is
            //multiplied by deltaTime and speed for a smooth MovePosition
            m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * playerSpeed);
        }
    }
}