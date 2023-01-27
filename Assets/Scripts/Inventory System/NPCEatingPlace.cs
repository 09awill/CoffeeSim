using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Inventory))]
public class NPCEatingPlace : MonoBehaviour
{
    [SerializeField] private bool m_DebugTestBool = false;
    [SerializeField] private Transform m_SeatTransform;
    Inventory m_Inventory;
    private bool m_Taken;
    private NPCController m_NPC;
    private void OnValidate()
    {
        if (!m_SeatTransform) m_SeatTransform = transform;
    }
    private void OnEnable()
    {
        m_Inventory = GetComponent<Inventory>();
        m_Inventory.AddFilter(CanHoldObjectType);
        m_Inventory.OnInventoryItemAdded += GiveNPCOrder;
    }

    private void GiveNPCOrder()
    {
        if (m_NPC)
        {
            var order = m_Inventory.GetListOfItems().Select(item => item.GetComponent<ConsumableContainer>()).Where(item => item != null).First();
            m_NPC.GiveOrder(order);
        }
    }

    public bool CanHoldObjectType(PickupableObject pObject)
    {
        if (m_DebugTestBool) return true;
        ConsumableContainer container = pObject.GetComponent<ConsumableContainer>();
        return (container && m_NPC && m_NPC.CheckOrderIsCorrect(container));
    }

    public bool IsAvailable()
    {
        return !m_Taken;
    }
    public Transform SeatNPC(NPCController pNPC)
    {
        m_Taken = true;
        m_NPC = pNPC;
        m_NPC.OnOrderChanged += OnOrderChanged;
        m_NPC.OnLeavingTable += OnNPCLeft;
        return m_SeatTransform ? m_SeatTransform : null;
    }

    private void OnNPCLeft()
    {
        m_NPC.OnOrderChanged -= OnOrderChanged;
        m_NPC.OnLeavingTable -= OnNPCLeft;
        m_NPC = null;
        CheckIfPlatesCanClear();
    }
    private void CheckIfPlatesCanClear()
    {
        Inventory parentInventory = m_Inventory.GetParentInventory();
        parentInventory.OnInventoryFilterUpdate += TryClearUpPlates;
        parentInventory.OnInventoryItemRemoved += TryClearUpPlates;
        TryClearUpPlates();
    }

    private void TryClearUpPlates()
    {
        Inventory parentInventory = m_Inventory.GetParentInventory();
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (parentInventory.CanHoldItem(items[i]))
            {
                parentInventory.TryPlaceItem(m_Inventory.PickupItem(i));
            }
        }
        if (items.Count <= 0)
        {
            m_Taken = false;
            parentInventory.OnInventoryFilterUpdate -= TryClearUpPlates;
            parentInventory.OnInventoryItemRemoved -= TryClearUpPlates;
        }
    }
    public void OnOrderChanged()
    {
        m_Inventory.UpdateFilters();
    }
#if UNITY_EDITOR
    private bool m_DebugTestLastFrame = false;
    private void Update()
    {
        if(m_DebugTestBool != m_DebugTestLastFrame)
        {
            m_Inventory.UpdateFilters();
            m_DebugTestLastFrame = m_DebugTestBool;
        }
    }
#endif

    private void OnDisable()
    {
        m_Inventory?.RemoveFilter(CanHoldObjectType);
    }
    private void OnDestroy()
    {
        m_Inventory?.RemoveFilter(CanHoldObjectType);
    }
}
