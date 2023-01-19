using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableContainer : PickupableObject
{
    public List<Consumable> m_Consumables = new List<Consumable>();
    private bool m_Dirty = false;
    public override PickupableObject Pickup()
    {
        return this;
    }
    public override PickupableObject Place()
    {
        return this;
    }

    public bool AddItem(PickupableObject pObject)
    {
        Consumable consumable = pObject as Consumable;
        if (consumable == null) return false;
        if (m_Consumables.Contains(consumable)) return false;
        m_Consumables.Add(consumable);
        pObject.transform.position = transform.position;
        pObject.transform.rotation = transform.rotation;
        pObject.transform.parent = transform;
        return true;
    }
    public List<SO_Consumable> GetConsumableData()
    {
        List<SO_Consumable> consumableData = new List<SO_Consumable>();
        foreach (Consumable consumable in m_Consumables)
        {
            consumableData.Add(consumable.GetConsumableData());
        }
        return consumableData;
    }
    public bool IsFull()
    {
        return m_Consumables.Count > 0;
    }

    internal bool IsClearAndClean()
    {
        return !IsFull() && !IsDirty();
    }

    public void Consume()
    {
        foreach (var consumable in m_Consumables)
        {
            consumable.Consume();
        }
        m_Dirty = true;
    }
    public void Clean()
    {
        m_Dirty = false;
        foreach (var consumable in m_Consumables)
        {
            Destroy(consumable.gameObject);
        }
        m_Consumables.Clear();
    }
    public bool IsDirty()
    {
        return m_Dirty;
    }
}
