using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SwitchPanel : MonoBehaviourPun
{
    private const byte PRESSURE_PAD = 1;

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
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EventCodes.PRESSURE_PAD, null, options, SendOptions.SendUnreliable);
    }
}
