using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventCodes : MonoBehaviour
{
    public static PhotonEventCodes instance;
    public enum EventCodes
    {
        CAGE = 0,
        PRESSURE_PAD = 1
    }
}
