using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Countertop : MonoBehaviour, IAcceptItem
{
    [SerializeField] private Transform m_ItemLocation;
    [SerializeField] private Rigidbody m_RB;
    private PickupableObject m_HeldItem;
    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }
    public bool PlaceItem(PickupableObject pObject)
    {
        if (m_HeldItem) return false;
        m_HeldItem = pObject;
        pObject.transform.position = m_ItemLocation.position;
        pObject.transform.rotation = m_ItemLocation.rotation;
        pObject.transform.parent = transform;
        return true;
    }

    public PickupableObject PickupItem()
    {
        if (!m_HeldItem) return null;
        PickupableObject item = m_HeldItem;
        m_HeldItem = null;
        return item;
    }
}
