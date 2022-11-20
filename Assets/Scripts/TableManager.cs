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
            if (!m_Instance) m_Instance = FindObjectOfType<TableManager>();
            if (!m_Instance) m_Instance = new TableManager();
            return m_Instance;
        }
        set
        {
            if (!m_Instance)
            {
                m_Instance = value;
            }
            else if(m_Instance != value)
            {
                Debug.LogWarning("Too many instances of table manager");
                Destroy(value.gameObject);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public Table GetTable()
    {
        if (m_Tables.Count < 0) return null;
        Table table = null;
        while (table == null || table.IsTaken() == true)
        {
            table = m_Tables[Random.Range(0, m_Tables.Count - 1)];
        }

        return table;
    }

    public void AddTable(Table pTable)
    {
        m_Tables.Add(pTable);   
    }
    
}
