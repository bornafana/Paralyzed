using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;


public class SwitchController : Interactable
{
    public SwitchPanel switchPanel;
    public bool switchOn;
    [HideInInspector]public Renderer switchColor;

    private void Start()
    {
        switchColor = GetComponent<Renderer>();
        SetSwitchAndColor();
        switchPanel.CheckSwitches();
    }

    public override void DoAction()
    {
        photonView.RPC("RPC_ChangeSwitchRotation", RpcTarget.All);
        photonView.RPC("RPC_SetSwitchAndColor", RpcTarget.All);
        photonView.RPC("RPC_CheckSwitches", RpcTarget.All);
    }

    private void ChangeSwitchRotation()
    {
        if (transform.parent.rotation.x > 0)
        {
            transform.parent.rotation = Quaternion.Euler(new Vector3(-30f, 0f, 0f));
        }
        else if (transform.parent.rotation.x < 0)
        {
            transform.parent.rotation = Quaternion.Euler(new Vector3(30f, 0f, 0f));
        }
    }

    private void SetSwitchAndColor()
    {
        if (transform.parent.rotation.x > 0)
        {
            switchColor.material.color = Color.green;
            switchOn = true;
        }
        else if (transform.parent.rotation.x < 0)
        {
            switchColor.material.color = Color.red;
            switchOn = false;
        }
    }

    [PunRPC]
    private void RPC_ChangeSwitchRotation()
    {
        ChangeSwitchRotation();
    }

    [PunRPC]
    private void RPC_SetSwitchAndColor()
    {
       SetSwitchAndColor();
    }

    [PunRPC]
    private void RPC_CheckSwitches()
    {
        switchPanel.CheckSwitches();
    }
}
