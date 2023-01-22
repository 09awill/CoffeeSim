using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item Supplier Class is used to spawn a specified item and store a specified amount. It will instantiate enough items to fill the array containing where the items should go.
/// Can be overridden to only allow specific items to be held
/// </summary>
public class ItemSupplier : Inventory
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
        return pObject != null;
    }

}
