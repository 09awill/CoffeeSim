using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Need to adjust this script to be more component based. Currently uses basically the same code as the sink so doesn't need to be two scripts
/// </summary>

public class CoffeeMachine : InteractableInventory, IInteractable
{
    [SerializeField] private Consumable m_Coffee;

    public override bool CanStartInteract()
    {
        if (m_HeldItems.Count < 1) return false;
        ConsumableContainer container = m_HeldItems[0] as ConsumableContainer;
        if (!container.IsClearAndClean()) return false;
        return base.CanStartInteract();
    }

    public override void OnInteract()
    {
        if (!m_Coffee) return;
        foreach(var item in m_HeldItems)
        {
            ConsumableContainer container = item as ConsumableContainer;
            Consumable consumable = Instantiate(m_Coffee, container.gameObject.transform.position, container.gameObject.transform.rotation);
            container.AddItem(consumable);
        }
        base.OnInteract();
    }
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer cup = pObject as ConsumableContainer;
        if (cup == null) return false;
        if (m_HeldItems.Count <= 0) return true;
        ConsumableContainer item = m_HeldItems[0] as ConsumableContainer;
        if (item.IsClearAndClean() != cup.IsClearAndClean()) return false;
        return true;
    }
}
