using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStyle : MonoBehaviourPunCallbacks
{
    public Material redMaterial;
    public Material greenMaterial;
    private void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<Renderer>().material = greenMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = redMaterial;
        }
    }
}
