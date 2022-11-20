using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public abstract void StartInteract();
    public abstract void StopInteract();

    public abstract void OnInteract();
}
