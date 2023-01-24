using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableObject: MonoBehaviour
{
    public abstract PickupableObject Pickup();
    public abstract PickupableObject Place();

    internal ConsumableContainer ToList()
    {
        throw new NotImplementedException();
    }
}
