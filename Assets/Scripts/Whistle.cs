using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Whistle : MonoBehaviourPun
{
    private const byte WHISTLE = 0;

    public AudioSource source;
    public AudioClip clipToPlay;

    void Start()
    {
        if (!photonView.IsMine || !PhotonNetwork.NickName.ToLower().Trim().Contains("mute"))
            enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC("RPC_Whistle", RpcTarget.All, "whistle");
        }
    }

    void PlaySound()
    {
        if (!PhotonNetwork.NickName.ToLower().Trim().Contains("deaf"))
            source.PlayOneShot(clipToPlay);
    }

    [PunRPC]
    void RPC_Whistle(string message)
    {
        PlaySound();
    }
}