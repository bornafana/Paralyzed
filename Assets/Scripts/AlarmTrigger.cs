﻿using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AlarmTrigger : Interactable
{
    private const byte WHISTLE = 0;

    public AudioSource source;
    public AudioClip clipToPlay;
    public override void DoAction()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_Whistle", RpcTarget.All, "whistle");
        }
    }

    void PlaySound()
    {
        if(!PhotonNetwork.NickName.ToLower().Contains("deaf"))
            source.PlayOneShot(clipToPlay);
    }

    [PunRPC]
    void RPC_Whistle(string message)
    {
        PlaySound();
    }
}
