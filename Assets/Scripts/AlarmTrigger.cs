using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AlarmTrigger : Interactable
{
    public AudioSource source;
    public AudioClip clipToPlay;
    public override void DoAction()
    {
        PhotonNetwork.SetSendingEnabled(2, false);
        photonView.RPC("PlaySound", RpcTarget.All);
    }

    [PunRPC]
    void PlaySound()
    {
        source.PlayOneShot(clipToPlay);
    }
}
