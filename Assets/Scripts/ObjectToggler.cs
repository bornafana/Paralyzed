using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ObjectToggler : MonoBehaviourPun
{
    [PunRPC]
    private void RPC_ToggleObject(bool toggleValue)
    {
        gameObject.SetActive(toggleValue);
    }

    public void ToggleObject(bool toggleValue)
    {
        //Adding photonview.ismine will make ONLY the host able to toggle on/off because the object originally instantiated on his scene.
        photonView.RPC("RPC_ToggleObject", RpcTarget.All, toggleValue);
    }
}
