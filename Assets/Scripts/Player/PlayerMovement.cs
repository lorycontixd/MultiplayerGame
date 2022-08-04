using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Lore.Stats;
using System;

/*[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    Player player;
    Rigidbody m_Rigidbody;

    public float basePushForce = 15f;
    [Range(2.8f, 7f)]
    public float jumpHeight = 5.2f;
    [Range(0.18f, 0.5f)]
    public float jumpDuration = 0.27f;

    public float playerSpeed;
    private bool canMove;
    private bool isGrounded;
    private bool canJump;
    private float jumpTimer;
    private bool jumpTimerActive = false;

    void Start()
    {
        //Fetch player component where all stats lie
        player = GetComponent<Player>();
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        canMove = photonView.IsMine;
        playerSpeed = player.MoveSpeed.Value;
        m_Rigidbody.mass = player.Mass;
        isGrounded = true;
        canJump = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) {
            return;
        }
        Debug.Log($"Colliding with {collision.collider.name}");
        if ( collision.collider.tag == "Player")
        {

            int enemyView = collision.gameObject.GetComponent<Player>().photonView.ViewID;
            PunEventSender.Instance.SendForce(enemyView, collision.transform.position - transform.position, CalculatorManager.Instance.CalculatePushForce(basePushForce, playerSpeed, player.Mass));
        }
        if (collision.collider.tag == "Ground" && !isGrounded)
        {
            isGrounded = true;
            canJump = true;
            jumpTimerActive = false;
            jumpTimer = 0f;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Left collision with {collision.collider.name}");
        if (collision.collider.tag == "Ground" && isGrounded)
        {
            isGrounded = false;
            jumpTimerActive = true;
        }
    }

    private void Update()
    {
        if (jumpTimerActive)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpDuration)
            {
                canJump = false;
            }
        }
        if (!isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.down * jumpTimer * 9.81f, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            playerSpeed = player.MoveSpeed.Value;
            //Store user input as a movement vector
            Vector3 m_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //Apply the movement vector to the current position, which is
            //multiplied by deltaTime and speed for a smooth MovePosition
            m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * playerSpeed);
            float jump = Input.GetAxis("Jump");
            if (jump > 0 && canJump)
            {
                m_Rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }
}
*/

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public Player player;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private bool canMove;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        player = GetComponent<Player>();
        controller = gameObject.AddComponent<CharacterController>();
        playerSpeed = player.MoveSpeed.Value;
        canMove = true;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            if (canMove)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime * playerSpeed);

                if (move != Vector3.zero)
                {
                    gameObject.transform.forward = move;
                }

                // Changes the height position of the player..
                if (Input.GetButtonDown("Jump") && groundedPlayer)
                {
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                }

                playerVelocity.y += gravityValue * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);
            }
        }    }
}