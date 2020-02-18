using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Cage : MonoBehaviourPun
{
    private Animator animator;
    public bool playerInCage;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //public void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == (byte)PhotonEventCodes.EventCodes.DEAFCAGEDOWN)
    //    {
    //        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageDown"))
    //            animator.SetTrigger("CageDown");
    //    }
    //    if (photonEvent.Code == (byte)PhotonEventCodes.EventCodes.DEAFCAGEUP)
    //    {
    //        if (playerInCage)
    //        {
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageUp"))
    //                animator.SetTrigger("CageUp");
    //        }
    //        else
    //        {
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageMaxUp"))
    //                animator.SetTrigger("CageMaxUp");
    //        }

    //    }
    //}

    [PunRPC]
    public void RPC_CageDown()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageDown"))
            animator.SetTrigger("CageDown");
    }

    [PunRPC]
    public void RPC_CageUp()
    {
        if (playerInCage)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageUp"))
                animator.SetTrigger("CageUp");

        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CageMaxUp"))
                animator.SetTrigger("CageMaxUp");

        }
    }

    public void CageDown()
    {
        photonView.RPC("RPC_CageDown", RpcTarget.All);
    }

    public void CageUp()
    {
        photonView.RPC("RPC_CageUp", RpcTarget.All);
    }

    //private void OnEnable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived += OnEvent;

    //}

    //private void OnDisable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    //}

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInCage = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInCage = false;
        }
    }
}
