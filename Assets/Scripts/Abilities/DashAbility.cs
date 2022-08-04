using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dash Ability", menuName = "Abilities/Dash Ability")]
public class DashAbility : Ability
{
    public float dashVelocity;

    public override void Fire(GameObject parent)
    {
        Debug.Log("Firing dash ability");
        Player player = parent.GetComponent<Player>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();

        rb.AddForce(parent.transform.forward * player.MoveSpeed.Value * dashVelocity, ForceMode.Impulse);
    }

    public override void Initialize()
    {
    }
}
