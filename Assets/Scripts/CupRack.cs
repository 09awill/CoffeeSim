using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupRack : Inventory
{
    [SerializeField] private PickupableObject m_ItemToSupply;
    private void Awake()
    {
        for(int i = 0; i < m_ModelLocations.Length; i++)
        {
            m_HeldItems.Add(Instantiate(m_ItemToSupply, m_ModelLocations[i].transform.position, m_ModelLocations[i].transform.localRotation, transform));
            RefreshModel();
        }
    }
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer plate = pObject as ConsumableContainer;
        if (plate == null || !plate.IsClearAndClean()) return false;
        return true;
    }

}
