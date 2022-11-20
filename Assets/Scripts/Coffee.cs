using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coffee : PickupableObject
{
    public override PickupableObject Pickup()
    {
        return this;
    }

    public override PickupableObject Place()
    {
        return this;
    }
}
