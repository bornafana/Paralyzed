using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {

        }
    }
}
