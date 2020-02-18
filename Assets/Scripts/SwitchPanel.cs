using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SwitchPanel : MonoBehaviourPun
{
    private enum Pad
    {
        DEAF,
        MUTE
    }

    private Pad pressurePadToActivate;
    public PressurePad padToActivate;
    public List<SwitchController> switches;
    public List<bool> correctPattern;
    public List<bool> switchPattern = new List<bool>();

    private bool correct = false;

    public void CheckSwitches()
    {
        switchPattern.Clear();

        for (int i = 0; i < switches.Count; i++)
        {
            switchPattern.Add(switches[i].switchOn);
        }

        if (correctPattern.SequenceEqual(switchPattern))
        {
            foreach (SwitchController contr in switches)
            {
                contr.switchColor.material.color = Color.yellow;
                Destroy(contr);
                EnablePressurePadEvent();
            }
        }
    }

    private void EnablePressurePadEvent()
    {
        //RPC
        padToActivate.ActivatePad();


        // EVENT
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        switch (pressurePadToActivate)
        {
            case Pad.DEAF:
                PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.PRESSURE_PAD_DEAF, null, options, SendOptions.SendUnreliable);
            break;

            case Pad.MUTE:
                PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.PRESSURE_PAD_MUTE, null, options, SendOptions.SendUnreliable);
                break;
        }
    }
}
