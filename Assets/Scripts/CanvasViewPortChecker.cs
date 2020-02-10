using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasViewPortChecker : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(GetComponent<PhotonView>().IsMine);
    }
}
