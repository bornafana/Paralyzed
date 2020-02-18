using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTogglerMessenger : Interactable
{
    public bool toggleOnTrigger = true;
    private bool toggle = false;
    public ObjectToggler objectToDestroy;

    public override void DoAction()
    {
        objectToDestroy.ToggleObject(toggle);
        toggle = !toggle;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Toggler") && toggleOnTrigger)
        {
            objectToDestroy.ToggleObject(toggle);
            toggle = !toggle;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Toggler") && toggleOnTrigger)
        {
            objectToDestroy.ToggleObject(toggle);
            toggle = !toggle;
        }
    }
}
