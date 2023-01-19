using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupRack : ItemSupplier
{
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer obj = pObject as ConsumableContainer;
        if (obj == null || !obj.IsClearAndClean()) return false;
        return base.CanHoldObjectType(pObject);
    }
}
