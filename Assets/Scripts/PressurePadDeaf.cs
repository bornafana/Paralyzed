using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadDeaf : MonoBehaviourPun
{
    public Animator animator;
    public Material materialWhenActive;
    private bool active = false;

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)PhotonEventCodes.EventCodes.PRESSURE_PAD_DEAF)
        {
            active = true;
            GetComponent<Renderer>().material = materialWhenActive;
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;

    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && active)
        {
            DropCage();
            animator.SetBool("PlayerOnPad", true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && active)
        {
            RaiseCage();
            animator.SetBool("PlayerOnPad", false);
        }
    }

    private void DropCage()
    {
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.DEAFCAGEDOWN, null, options, SendOptions.SendUnreliable);
    }

    private void RaiseCage()
    {
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.DEAFCAGEUP, null, options, SendOptions.SendUnreliable);
    }
}