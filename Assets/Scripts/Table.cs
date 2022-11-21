using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Table : MonoBehaviour, IAcceptItem
{
    [SerializeField] private Chair[] m_Chairs;
    [SerializeField] private Transform m_HeldItemsLocation;
    [SerializeField] private Rigidbody m_RB;
    private bool m_Taken = false;
    private List<PickupableObject> m_HeldObjects = new List<PickupableObject>();
    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
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
        }

        return chair;
    }

    public bool PlaceItem(PickupableObject pObject)
    {
        m_HeldObjects.Add(pObject);
        pObject.transform.position = m_HeldItemsLocation.position;
        pObject.transform.rotation = m_HeldItemsLocation.rotation;
        pObject.transform.parent = transform;
        return true;
    }

    public PickupableObject PickupItem()
    {
        int count = m_HeldObjects.Count;
        if (count < 1) return null;
        PickupableObject item = m_HeldObjects[count - 1];
        m_HeldObjects.Remove(item);
        return item;
    }
    private Consumable GetFirstConsumablePlaced()
    {
        foreach (PickupableObject obj in m_HeldObjects)
        {
            Consumable consumable = obj as Consumable;
            if(consumable != null) return consumable;
        }
        return null;
    }

    private void Update()
    {
        for (int i = 0; i < m_Chairs.Length; i++)
        {
            if (m_Chairs[i].NPC && m_Chairs[i].NPC.GetOrder())
            {
                if (m_HeldObjects.Count > 0)
                {
                    m_Chairs[i].GiveOrder(GetFirstConsumablePlaced());
                    m_HeldObjects.RemoveAt(0);
                }
            } else if(m_Chairs[i].NPC && m_Chairs[i].NPC.GetFinished())
            {
                m_HeldObjects.Add(m_Chairs[i].TakePlate());
                Consumable consumable = m_HeldObjects[m_HeldObjects.Count - 1] as Consumable;
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
        public Consumable Order;
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
        public void GiveOrder(Consumable pOrder)
        {
            Order = pOrder;
            NPC.GiveOrder();
        }
    }
}
