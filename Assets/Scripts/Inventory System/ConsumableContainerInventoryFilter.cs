using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Inventory))]
public class ConsumableContainerInventoryFilter : MonoBehaviour
{
    Inventory m_Inventory;
    private void OnEnable()
    {
        m_Inventory = GetComponent<Inventory>();
        m_Inventory?.AddFilter(CanHoldObjectType);
    }
    public bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer c = pObject.GetComponent<ConsumableContainer>();
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

    private void OnDisable()
    {
        m_Inventory?.RemoveFilter(CanHoldObjectType);
    }
    private void OnDestroy()
    {
        m_Inventory?.RemoveFilter(CanHoldObjectType);
    }
}
