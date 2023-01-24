using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Inventory))]
public class ConsumableContainerInventoryFilter : MonoBehaviour
{
    private void Awake()
    {
        Inventory inventory = GetComponent<Inventory>();
        inventory.AddFilter(CanHoldObjectType);
    }
    public bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer c = pObject as ConsumableContainer;
        if (c == null)
        {
            return false;
        }
        if (!c.IsClearAndClean())
        {
            return false;
        }
        return true;
    }
}
