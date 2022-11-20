using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    private TableState m_TableState = TableState.NotAssignedTable;
    private Table m_TargetTable;
    private bool m_CoffeeOrder = false;
    private bool m_AtTable = false;
    private void Awake()
    {
        if (!m_NavMeshAgent) m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_TargetTable = TableManager.Instance.GetTable();
        m_TargetTransform.position = m_TargetTable.GetFreeChair(this).transform.position;
        m_TableState = TableState.GettingToTable;
    }

    private void Update()
    {
        m_NavMeshAgent.destination = m_TargetTransform.position;
        if (m_TableState == TableState.GettingToTable)
        {
            float distance = (m_NavMeshAgent.destination - transform.position).magnitude;
            if (distance < 0.1f)
            {
                m_TableState = TableState.AtTable;
                StartCoroutine(GetOrderCoroutine());
            }
        }
    }

    public bool GetOrder()
    {
        return m_CoffeeOrder;
    }

    public void GiveOrder()
    {
        m_CoffeeOrder = false;
    }
    private IEnumerator GetOrderCoroutine()
    {
        yield return new WaitForSeconds(5);
        m_CoffeeOrder = true;
        yield return null;
    }

    public enum TableState
    {
        NotAssignedTable,
        GettingToTable,
        AtTable,
        FinishedAtTable,
    };
}
