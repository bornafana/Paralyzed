using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviourPun
{
    public Animator animator;
    public Material materialWhenActive;
    private bool active = false;

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)PhotonEventCodes.EventCodes.PRESSURE_PAD)
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Player") && active)
        {
                DropCage();
                //animator.SetBool("PlayerOnPad", true);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.transform.CompareTag("Player") && active)
        {
            //animator.SetBool("PlayerOnPad", false);
        }
    }

    private void DropCage()
    {
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.CAGE, null, options, SendOptions.SendUnreliable);
    }
}