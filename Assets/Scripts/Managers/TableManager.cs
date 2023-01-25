using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Manager handles a list of tables which allows a static reference to find a place for NPCs
/// </summary>
public class TableManager : MonoBehaviour
{
    [SerializeField] private Transform m_Exit;
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
        var free = m_Tables.Where(t => !t.IsFull());
        Table table = free.Any() ? free.ElementAt(Random.Range(0, free.Count())) : null;
        return table; //free.Any() ? free.ElementAt(Random.Range(0, m_Tables.Count)) : null;
    }
    public Transform GetExit()
    {
        return m_Exit;
    }

    public void AddTable(Table pTable)
    {
        m_Tables.Add(pTable);   
    }
    
}
