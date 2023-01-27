using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Base script for containing consumables, designed to be used for plates, cups etc.
/// </summary>
public class ConsumableContainer: MonoBehaviour
{
    public List<Consumable> m_Consumables = new List<Consumable>();
    private bool m_Dirty = false;

    public bool AddItem(Consumable pConsumable)
    {
        if (pConsumable == null) return false;
        if (m_Consumables.Contains(pConsumable)) return false;
        m_Consumables.Add(pConsumable);
        pConsumable.transform.position = transform.position;
        pConsumable.transform.rotation = transform.rotation;
        pConsumable.transform.parent = transform;
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
