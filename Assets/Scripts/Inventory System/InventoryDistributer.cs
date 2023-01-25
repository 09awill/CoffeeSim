using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// Script initially designed for tables, passes items from inventory into referenced children inventories
/// </summary>
[RequireComponent(typeof(Inventory))]
public class InventoryDistributer : MonoBehaviour
{
    [SerializeField] private List<Inventory> m_ChildInventories= new List<Inventory>();
    private Inventory m_Inventory;
    private void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
        m_Inventory.OnInventoryItemAdded += CheckInventoryMatchesChildFilters;
        m_Inventory.OnInventoryFilterUpdate += CheckInventoryMatchesChildFilters;
        foreach(Inventory childInventory in m_ChildInventories)
        {
            childInventory.SetParent(m_Inventory);
            childInventory.OnInventoryItemRemoved += CheckInventoryMatchesChildFilters;
            childInventory.OnInventoryFilterUpdate += CheckInventoryMatchesChildFilters;
        }
    }

    private void CheckInventoryMatchesChildFilters()
    {
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        for(int i = items.Count -1; i >= 0; i--)
        {
            Inventory itemFitsFilter = m_ChildInventories.FirstOrDefault(inventory => inventory.CanHoldItem(items[i]));
            itemFitsFilter?.TryPlaceItem(m_Inventory.PickupItem(i));
        }
        
    }
}
