using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AlarmTrigger : Interactable
{
    private const byte WHISTLE = 1;

    public AudioSource source;
    public AudioClip clipToPlay;
    public override void DoAction()
    {
        //PhotonNetwork.SetSendingEnabled(2, false);
        object clip = clipToPlay;
        PhotonNetwork.RaiseEvent(WHISTLE, clip, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    [PunRPC]
    void PlaySound()
    {
        source.PlayOneShot(clipToPlay);
    }
}
