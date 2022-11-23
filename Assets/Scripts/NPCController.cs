using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private float m_EatTime = 1f;
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
        GetPlace();
    }

    private void GetPlace()
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
            if (distance < 2f)
            {
                m_TableState = TableState.AtTable;
                StartCoroutine(GetOrderCoroutine());
            }
        }
        if (m_TableState == TableState.FinishedAtTable)
        {
            float distance = (m_NavMeshAgent.destination - transform.position).magnitude;
            if (distance < 2f)
            {
                m_TableState = TableState.NotAssignedTable;
                GetPlace();
            }
        }
    }

    public bool GetOrder()
    {
        return m_CoffeeOrder;
    }

    public void GiveOrder()
    {
        StartCoroutine(Eat());
    }
    private IEnumerator Eat()
    {
        yield return new WaitForSeconds(m_EatTime);
        m_CoffeeOrder = false;
        m_TableState = TableState.FinishedAtTable;
        m_TargetTransform.position = TableManager.Instance.GetExit().position;
    }
    public bool GetFinished()
    {
        return m_TableState == TableState.FinishedAtTable;
    }
    private IEnumerator GetOrderCoroutine()
    {
        yield return new WaitForSeconds(5);
        m_CoffeeOrder = true;
        m_TableState = TableState.Eating;
        yield return null;
    }

    public enum TableState
    {
        NotAssignedTable,
        GettingToTable,
        AtTable,
        Eating,
        FinishedAtTable,
    };
}
