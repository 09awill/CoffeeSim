using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected Transform[] m_ModelLocations;
    protected List<PickupableObject> m_HeldItems = new List<PickupableObject>();
    public bool PlaceItem(PickupableObject pObject)
    {
        if (m_HeldItems.Count >= m_ModelLocations.Length) return false;
        if (!CanHoldObjectType(pObject)) return false;
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
    public virtual bool CanHoldObjectType(PickupableObject pObject)
    {
        if (pObject == null) return false;
        return true;
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
