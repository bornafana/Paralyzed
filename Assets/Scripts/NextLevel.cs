using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class NextLevel : Interactable
{
    private bool winCondition = false;
    public GameObject blueSeal;
    public GameObject redSeal;

    public override void DoAction()
    {
        winCondition = (!blueSeal.activeInHierarchy && !redSeal.activeInHierarchy);

        if(winCondition)
            photonView.RPC("RPC_LoadNextLevel", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_LoadNextLevel()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
