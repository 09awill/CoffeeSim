using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Inventory script allows user to place any object
/// </summary>
public class Inventory: MonoBehaviour
{
    [SerializeField] protected Transform[] m_ModelLocations;
    protected List<PickupableObject> m_HeldItems = new List<PickupableObject>();

    private delegate bool ItemFilter(PickupableObject pObject);
    private List<ItemFilter> m_Filters = new List<ItemFilter>();

    /// <summary>
    /// Adds Inventory Filter to delegate list
    /// </summary>
    /// <param name="pMethod">Method With Pickupable Object as a parameter and a boolean return type</param>
    public void AddFilter(Func<PickupableObject, bool> pMethod)
    {
        ItemFilter filter = new ItemFilter(pMethod);
        m_Filters.Add(filter);
    }
    public bool PlaceItem(PickupableObject pObject)
    {
        if (m_HeldItems.Count >= m_ModelLocations.Length) return false;
        if (pObject == null) return false;
        if (m_Filters.Count > 0)
        {
            bool fitsFilters = m_Filters.Where(filter => filter.Invoke(pObject) == true).Any();
            if (!fitsFilters) return false;
        }
        m_HeldItems.Add(pObject);
        RefreshModel();
        return true;
    }
    public PickupableObject PickupItem()
    {
        if (m_HeldItems.Count == 0) return null;
        PickupableObject item = m_HeldItems[0];
        m_HeldItems.RemoveAt(0);
        RefreshModel();
        return item;
    }
    public List<PickupableObject> GetListOfItems()
    {
        return m_HeldItems;
    }
    public int GetCapacity()
    {
        return m_ModelLocations.Length;
    }
    protected void RefreshModel()
    {
        for (int i = 0; i < m_HeldItems.Count; i++)
        {
            m_HeldItems[i].transform.position = m_ModelLocations[i].transform.position;
            m_HeldItems[i].transform.rotation = m_ModelLocations[i].transform.rotation;
            m_HeldItems[i].transform.parent = transform;
        }
    }
}
