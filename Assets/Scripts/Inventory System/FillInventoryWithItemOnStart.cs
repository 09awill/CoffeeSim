using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item Supplier Class is used to spawn a specified item and store a specified amount. It will instantiate enough items to fill the array containing where the items should go.
/// Can be overridden to only allow specific items to be held
/// </summary>
[RequireComponent(typeof(Inventory))]
public class FillInventoryWithItemOnStart : MonoBehaviour
{
    [SerializeField] private PickupableObject m_ItemToSupply;
    private void Awake()
    {
        if (m_ItemToSupply == null) return;
        Inventory inventory = GetComponent<Inventory>();
        for(int i = 0; i < inventory.GetCapacity(); i++)
        {
            inventory.TryPlaceItem(Instantiate(m_ItemToSupply));
        }
        enabled = false;
    }

}
