using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAcceptItem
{
    public bool PlaceItem(PickupableObject pObject);
    public PickupableObject PickupItem();
}
