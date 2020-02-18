using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyerMessenger : Interactable
{
    public ObjectDestroyer objectDestroyer;

    public override void DoAction()
    {
        objectDestroyer.DestroyObject();
    }
}
