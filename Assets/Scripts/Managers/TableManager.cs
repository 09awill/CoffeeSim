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
    #region Singleton
    private static TableManager m_Instance;
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
    #endregion

    [SerializeField] private Transform m_Exit;
    private List<Table> m_Tables = new List<Table>();


    public Table GetTable()
    {
        var free = m_Tables.Where(t => !t.IsFull());
        return free.Any() ? free.ElementAt(Random.Range(0, free.Count())) : null;
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
