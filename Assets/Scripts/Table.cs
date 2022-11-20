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
        pObject.AddComponent<FixedJoint>().connectedBody = m_RB;
        return true;
    }

    public PickupableObject PickupItem()
    {
        int count = m_HeldObjects.Count;
        if (count < 1) return null;
        Destroy(m_HeldObjects[count - 1].GetComponent<FixedJoint>());
        PickupableObject item = m_HeldObjects[count];
        m_HeldObjects.Remove(item);
        return item;
    }

    private void Update()
    {
        for (int i = 0; i < m_Chairs.Length; i++)
        {
            if (m_Chairs[i].NPC && m_Chairs[i].NPC.GetOrder())
            {
                if (m_HeldObjects.Count > 0)
                {
                    m_HeldObjects[0].gameObject.SetActive(false);
                    m_Chairs[i].NPC.GiveOrder();
                }
            }
        }
    }
    [Serializable]
    class Chair
    {
        public bool Available;
        public Transform Transform;
        public NPCController NPC;
    }
}
