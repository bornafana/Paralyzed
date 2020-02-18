using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ObjectDestroyer : MonoBehaviourPun
{
    [PunRPC]
    private void RPC_DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyObject()
    {
        //Adding photonview.ismine will make ONLY the host able to destroy objects because the object originally instantiated on his scene.
        photonView.RPC("RPC_DestroyObject", RpcTarget.All);
    }
}
