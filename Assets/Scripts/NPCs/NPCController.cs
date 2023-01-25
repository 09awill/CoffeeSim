using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Script is too big, could potentially seperate into NPC movement controller and NPC order controller
/// </summary>
public class NPCController : MonoBehaviour
{
    public Action OnOrderChanged;
    public Action OnEating;
    public Action OnLeavingTable;
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private float m_EatTime = 1f;
    [SerializeField] private bool m_DrawDebug = false;
    private TableState m_TableState = TableState.NotAssignedTable;
    private Table m_TargetTable;
    private List<Consumable> m_CoffeeOrder = null;
    private ConsumableContainer m_GivenOrder = null;
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
            Transform chair = m_TargetTable.SeatNPC(this);
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
                StartCoroutine(GetOrderCoroutine());
            }
        }
        if (m_TableState == TableState.FinishedAtTable)
        {
            if (HasReachedDestination())
            {
                m_TableState = TableState.NotAssignedTable;
                gameObject.SetActive(false);
                return;
            }
        }
        Vector3 lookDirection = HasReachedDestination() ? (m_TargetTransform.forward + m_TargetTransform.position) - transform.position : (transform.position + m_NavMeshAgent.velocity.normalized) - transform.position;
        if (lookDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
        }
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
        if (!CheckOrderIsCorrect(pOrder)) return false;
        m_GivenOrder = pOrder;
        StartCoroutine(Eat());
        return true;
    }
    public List<Consumable> GetOrder()
    {
        return m_CoffeeOrder;
    }
    public bool CheckOrderIsCorrect(ConsumableContainer pOrder)
    {
        if (m_CoffeeOrder == null) return false;
        if (pOrder == null) return false;
        if (pOrder.IsDirty()) return false;
        List<SO_Consumable> consumableData = pOrder.GetConsumableData();
        if (m_CoffeeOrder.Count != consumableData.Count) return false;
        if (m_CoffeeOrder.Where(consumable => !consumableData.Contains(consumable.GetConsumableData())).Any())
        {
            return false;
        }
        return true;
    }
    private IEnumerator Eat()
    {
        OnEating.Invoke();
        yield return new WaitForSeconds(m_EatTime);
        m_GivenOrder.Consume();
        m_GivenOrder = null;
        m_CoffeeOrder = null;
        OnOrderChanged.Invoke();
        m_TableState = TableState.FinishedAtTable;
        m_TargetTransform.position = TableManager.Instance.GetExit().position;
        OnLeavingTable.Invoke();
    }
    public bool GetFinished()
    {
        return m_TableState == TableState.FinishedAtTable;
    }
    private IEnumerator GetOrderCoroutine()
    {
        yield return new WaitForSeconds(5);
        m_CoffeeOrder = OrderManager.GetOrder();
        OnOrderChanged.Invoke();
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
        if(m_DrawDebug)Gizmos.DrawLine(transform.position, transform.position + m_TargetTransform.forward);
    }
    private void OnDestroy()
    {
        if(m_TargetTransform)Destroy(m_TargetTransform.gameObject);
    }
}
