using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Countertop : Inventory
{
    [SerializeField] private Transform m_ItemLocation;
    [SerializeField] private Rigidbody m_RB;
    private PickupableObject m_HeldItem;
    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }
}
