using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Cleaner script is an interactable which cleans items held in an inventory.
/// </summary>
[RequireComponent(typeof(Inventory))]
public class Cleaner : BaseInteractable
{
    private Inventory m_Inventory;

    private void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
    }

    public override void OnInteract()
    {
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        foreach (var item in items)       
        {
            ConsumableContainer container = item as ConsumableContainer;
            container.Clean();
        }
        base.OnInteract();
    }
    public override bool CanStartInteract()
    {
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        if (items.Count < 1) return false;
        bool isDirty = false;
        foreach (var item in items)
        {
            ConsumableContainer container = item as ConsumableContainer;
            if (!container.IsClearAndClean()) isDirty = true;
        }
        if (!isDirty) return false;
        return base.CanStartInteract();
    }

}
