using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PressurePad : MonoBehaviourPun
{
    public Cage cageToDrop;
    public Animator animator;
    public Material materialWhenActive;
    private bool active = true;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && active)
        {
            cageToDrop.CageDown();
            animator.SetBool("PlayerOnPad", true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && active)
        {
            cageToDrop.CageUp();
            animator.SetBool("PlayerOnPad", false);
        }
    }


    [PunRPC]
    public void RPC_ActivatePad()
    {
        active = true;
        GetComponent<Renderer>().material = materialWhenActive;
    }

    public void ActivatePad()
    {
        photonView.RPC("RPC_ActivatePad", RpcTarget.All);
    }
}