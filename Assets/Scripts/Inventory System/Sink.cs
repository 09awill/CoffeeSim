using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sink : InteractableInventory, IInteractable
{
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer plate = pObject as ConsumableContainer;
        if (plate == null) return false;
        if (m_HeldItems.Count <= 0) return true;
        ConsumableContainer item  = m_HeldItems[0] as ConsumableContainer;
        if (item.IsClearAndClean() != plate.IsClearAndClean()) return false;
        return true;
    }

    public override void OnInteract()
    {
        foreach (var item in m_HeldItems)       
        {
            ConsumableContainer container = item as ConsumableContainer;
            container.Clean();
        }
        base.OnInteract();
    }
    public override bool CanStartInteract()
    {
        if (m_HeldItems.Count < 1) return false;
        ConsumableContainer container = m_HeldItems[0] as ConsumableContainer;
        if (container.IsClearAndClean()) return false;
        return base.CanStartInteract();
    }
}
