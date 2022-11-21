using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : PickupableObject
{
    public override PickupableObject Pickup()
    {
        return this;
    }

    public override PickupableObject Place()
    {
        return this;
    }
    public abstract void Consume();
}
