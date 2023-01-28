using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Inventory script allows user to place any object
/// </summary>
public class Inventory: MonoBehaviour
{
    public Action OnInventoryItemAdded;
    public Action OnInventoryItemRemoved;
    public Action OnInventoryFilterUpdate;
    public Action OnInventoryTargeted;
    public Action OnInventoryUnTargeted;


    [SerializeField] private Transform[] m_ModelLocations;
    private List<PickupableObject> m_HeldItems = new List<PickupableObject>();
    private delegate bool ItemFilter(PickupableObject pObject);
    private List<ItemFilter> m_Filters = new List<ItemFilter>();

    //This allows inventorys chain together
    private Inventory m_ParentInventory = null;

    /// <summary>
    /// Adds Inventory Filter to delegate list
    /// </summary>
    /// <param name="pMethod">Method With Pickupable Object as a parameter and a boolean return type</param>
    public void AddFilter(Func<PickupableObject, bool> pMethod)
    {
        ItemFilter filter = new ItemFilter(pMethod);
        m_Filters.Add(filter);
        OnInventoryFilterUpdate?.Invoke();
    }
    public void SetAsTarget(PickupableObject pObject = null)
    {
        if(!pObject)
        {
            if (m_HeldItems.Count > 0)
            {
                OnInventoryTargeted?.Invoke(); 
            } else
            {
                OnInventoryUnTargeted?.Invoke();
            }
        } else
        {
            if (CanHoldItem(pObject))
            {
                OnInventoryTargeted?.Invoke();
            } else
            {
                OnInventoryUnTargeted?.Invoke();
            }
        }
    }
    public void RemoveAsTarget()
    {
        OnInventoryUnTargeted.Invoke();
    }
    public void UpdateFilters()
    {
        OnInventoryFilterUpdate?.Invoke();
    }
    public void RemoveFilter(Func<PickupableObject, bool> pMethod)
    {
        m_Filters.RemoveAll(filter => filter.Method.Equals(pMethod));
    }
    
    public void SetParent(Inventory pInventory)
    {
        m_ParentInventory = pInventory;
    }

    public Inventory GetParentInventory()
    {
        return m_ParentInventory;
    }

    public bool TryPlaceItem(PickupableObject pObject)
    {
        if (!CanHoldItem(pObject)) return false;
        m_HeldItems.Add(pObject);
        RefreshModel();
        OnInventoryItemAdded?.Invoke();
        return true;
    }
    public bool CanHoldItem(PickupableObject pObject)
    {
        if (m_HeldItems.Count >= m_ModelLocations.Length) return false;
        if (pObject == null) return false;
        if (m_Filters.Count > 0)
        {
            bool fitsFilters = m_Filters.Where(filter => filter.Invoke(pObject) == true).Any();
            if (!fitsFilters) return false;
        }
        return true;
    }
    public PickupableObject PickupItem(int pAtIndex = 0)
    {
        if (m_HeldItems.Count == 0 || m_HeldItems.Count - 1 < pAtIndex) return null;
        PickupableObject item = m_HeldItems[pAtIndex];
        m_HeldItems.RemoveAt(pAtIndex);
        RefreshModel();
        OnInventoryItemRemoved?.Invoke();
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
