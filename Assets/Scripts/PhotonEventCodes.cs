using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventCodes : MonoBehaviour
{
    public static PhotonEventCodes instance;
    public enum EventCodes
    {
        DEAFCAGEDOWN = 0,
        DEAFCAGEUP = 1,
        MUTECAGEDOWN = 2,
        MUTECAGEUP = 3,
        PRESSURE_PAD_DEAF = 4,
        PRESSURE_PAD_MUTE = 5
    }
}
