using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    public abstract void DoAction();
}