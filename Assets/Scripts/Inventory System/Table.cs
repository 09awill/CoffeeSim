using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Table : Inventory
{
    [SerializeField] private Chair[] m_Chairs;
    private bool m_Taken = false;
    private void Awake()
    {
        TableManager.Instance.AddTable(this);
    }

    public bool IsTaken()
    {
        return m_Taken;
    }

    public Transform GetFreeChair(NPCController pNPC)
    {
        Transform chair = null;
        while (chair == null)
        {
            Chair option = m_Chairs[Random.Range(0, m_Chairs.Length - 1)];
            if (!option.Available) continue;
            chair = option.Transform;
            option.Available = false;
            option.NPC = pNPC;
            m_Taken = true;
        }

        return chair;
    }
    private ConsumableContainer GetFirstConsumableContainerPlaced()
    {
        foreach (PickupableObject obj in m_HeldItems)
        {
            ConsumableContainer consumableContainer = obj as ConsumableContainer;
            if(consumableContainer != null) return consumableContainer;
        }
        return null;
    }

    private void Update()
    {
        for (int i = 0; i < m_Chairs.Length; i++)
        {
            if (m_Chairs[i].NPC && m_Chairs[i].NPC.GetIsOrdering())
            {
                if (m_HeldItems.Count > 0)
                {
                    if(!m_Chairs[i].GiveOrder(GetFirstConsumableContainerPlaced())) continue;
                    m_HeldItems.RemoveAt(0);
                }
            } else if(m_Chairs[i].NPC && m_Chairs[i].NPC.GetFinished())
            {
                m_HeldItems.Add(m_Chairs[i].TakePlate());
                ConsumableContainer consumable = m_HeldItems[m_HeldItems.Count - 1] as ConsumableContainer;
                consumable.Consume();
                m_Chairs[i].ClearUpSeat();
            }
        }
    }
    [Serializable]
    class Chair
    {
        public bool Available;
        public Transform Transform;
        public NPCController NPC;
        public ConsumableContainer Order;
        public void ClearUpSeat()
        {
            Available = true;
            NPC = null;
        }
        public PickupableObject TakePlate()
        {
            PickupableObject obj = Order;
            Order = null;
            return obj;
        }
        public bool GiveOrder(ConsumableContainer pOrder)
        {
            if (!NPC.GiveOrder(pOrder)) return false;
            Order = pOrder;
            return true;
        }
    }
}
