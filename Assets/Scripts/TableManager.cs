using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TableManager : MonoBehaviour
{
    private static TableManager m_Instance;
    private List<Table> m_Tables = new List<Table>();

    public static TableManager Instance {
        get
        {
            return m_Instance;
        }
    }
    private void Awake()
    {
        if (m_Instance != null)
        {
            Debug.LogWarning("Too many instances of table manager");
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public Transform GetChair()
    {
        Transform chair = null;
        while (chair == null)
        {
            chair = m_Tables[Random.Range(0, m_Tables.Count - 1)].GetFreeChair();
        }

        return chair;
    }

    public void AddTable(Table pTable)
    {
        m_Tables.Add(pTable);   
    }
    
}
