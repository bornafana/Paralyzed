using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviourPun
{
    private const byte CAGE_DROP = 0;

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
                DropCage();
        }
    }

    private void DropCage()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        GetComponent<Renderer>().material.color = new Color(r, g, b);

        object[] datas = new object[] { r, g, b };
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent(CAGE_DROP, null, options, SendOptions.SendUnreliable);
    }
}