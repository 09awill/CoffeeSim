using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<Consumable> m_Consumables = new List<Consumable>();
    private static OrderManager m_Instance;

    public static OrderManager Instance
    {
        get
        {
            if (!m_Instance) m_Instance = FindObjectOfType<OrderManager>();
            if (!m_Instance) m_Instance = new OrderManager();
            return m_Instance;
        }
        set
        {
            if (!m_Instance)
            {
                m_Instance = value;
            }
            else if (m_Instance != value)
            {
                Debug.LogWarning("Too many instances of order manager");
                Destroy(value.gameObject);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    public static List<Consumable> GetOrder()
    {
        List<Consumable> list = new List<Consumable>();
        List<Consumable> mainList = Instance.m_Consumables;
        list.Add(mainList[Random.Range(0, mainList.Count)]);
        return list;
    }
}