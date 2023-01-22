using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Script to add consumable to container on interact
/// </summary>
[RequireComponent(typeof(Inventory))]
public class CoffeeMachine : BaseInteractable
{
    [SerializeField] private Consumable m_Coffee;
    private Inventory m_Inventory;

    private void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
    }
    public override bool CanStartInteract()
    {
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        if (items.Count < 1) return false;
        bool allContainCoffee = true;
        foreach (var item in items)
        {
            ConsumableContainer container = item as ConsumableContainer;
            List<SO_Consumable> consumables = container.GetConsumableData();
            if (!consumables.Contains(m_Coffee.GetConsumableData())) allContainCoffee = false;
        }
        if (allContainCoffee) return false;
        return base.CanStartInteract();
    }

    public override void OnInteract()
    {
        if (!m_Coffee) return;
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        foreach (var item in items)
        {
            ConsumableContainer container = item as ConsumableContainer;
            List<SO_Consumable> consumables = container.GetConsumableData();
            bool shouldFill = false;
            if (!consumables.Contains(m_Coffee.GetConsumableData())) shouldFill = true;
            if (shouldFill)
            {
                Consumable consumable = Instantiate(m_Coffee, container.gameObject.transform.position, container.gameObject.transform.rotation);
                container.AddItem(consumable);
            }
        }
        base.OnInteract();
    }
}
