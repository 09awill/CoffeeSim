using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private float m_EatTime = 1f;
    private TableState m_TableState = TableState.NotAssignedTable;
    private Table m_TargetTable;
    private List<Consumable> m_CoffeeOrder = null;
    private bool m_AtTable = false;
    private void Awake()
    {
        if (!m_NavMeshAgent) m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Initialise(Transform pTransform)
    {
        m_TargetTransform = Instantiate(pTransform, pTransform.position, pTransform.rotation);
        StartCoroutine(GetPlace());
    }

    private IEnumerator GetPlace()
    {
        while (m_TableState != TableState.GettingToTable)
        {
            m_TargetTable = TableManager.Instance.GetTable();
            if (m_TargetTable == null)
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            Transform chair = m_TargetTable.GetFreeChair(this);
            if (chair == null)
            {
                            yield return new WaitForSeconds(1);
                continue;
            }
            m_TargetTransform.SetPositionAndRotation(chair.position, chair.rotation);
            m_TableState = TableState.GettingToTable;
        }
        yield return null;
    }
    private void Update()
    {
        if(m_NavMeshAgent.transform.position != m_TargetTransform.position) m_NavMeshAgent.destination = m_TargetTransform.position;
        if (m_TableState == TableState.GettingToTable)
        {
            if (HasReachedDestination())
            {
                m_TableState = TableState.AtTable;
                //transform.LookAt(transform.position + m_TargetTransform.forward);
                StartCoroutine(RotateToFace());
                StartCoroutine(GetOrderCoroutine());
            }
        }
        if (m_TableState == TableState.FinishedAtTable)
        {
            if (HasReachedDestination())
            {
                m_TableState = TableState.NotAssignedTable;
                gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator RotateToFace()
    {
        var targetRotation = Quaternion.LookRotation(m_TargetTransform.transform.position - transform.position);
        while (!Quaternion.Equals(transform.rotation, targetRotation))
        {
            targetRotation = Quaternion.LookRotation((transform.position + m_TargetTransform.transform.forward) - transform.position);
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
    public bool HasReachedDestination()
    {
        if (!m_NavMeshAgent.pathPending)
        {
            if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
            {
                if (!m_NavMeshAgent.hasPath || m_NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool GetIsOrdering()
    {
        return m_CoffeeOrder != null;
    }

    public bool GiveOrder(ConsumableContainer pOrder)
    {
        if (pOrder.IsDirty()) return false;
        List<SO_Consumable> consumableData = pOrder.GetConsumableData();
        
        if (m_CoffeeOrder.Count != consumableData.Count) return false;
        foreach (Consumable c in m_CoffeeOrder)
        {
            if (!consumableData.Contains(c.GetConsumableData())) return false;
        }
        StartCoroutine(Eat());
        return true;
    }
    private IEnumerator Eat()
    {
        yield return new WaitForSeconds(m_EatTime);
        m_CoffeeOrder = null;
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
        m_CoffeeOrder = OrderManager.GetOrder();
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + m_TargetTransform.forward);
    }
    private void OnDestroy()
    {
        if(m_TargetTransform)Destroy(m_TargetTransform.gameObject);
    }
}
