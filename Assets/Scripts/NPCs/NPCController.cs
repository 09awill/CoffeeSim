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
[RequireComponent(typeof(CharacterMovement))]
public class NPCController : MonoBehaviour
{
    public Action OnOrderChanged;
    public Action OnEating;
    public Action OnLeavingTable;
    [SerializeField] private float m_EatTime = 1f;
    [SerializeField] private CharacterMovement m_CharacterMovement;
    private TableState m_TableState = TableState.NotAssignedTable;
    private Table m_TargetTable;
    private List<Consumable> m_CoffeeOrder = null;
    private ConsumableContainer m_GivenOrder = null;
    private void Awake()
    {
        m_CharacterMovement = GetComponent<CharacterMovement>();
    }
    private void Start()
    {
        m_CharacterMovement.SetTargetPositionAndRot(transform.position, transform.rotation);
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
            m_CharacterMovement.SetTargetPositionAndRot(chair.position, chair.rotation);
            m_TableState = TableState.GettingToTable;
            m_CharacterMovement.OnArrivedAtDestination += ReachedTable;
        }
        yield return null;
    }

    private void ReachedTable()
    {
        m_TableState = TableState.AtTable;
        StartCoroutine(GetOrderCoroutine());
        m_CharacterMovement.OnArrivedAtDestination -= ReachedTable;
    }
    private void ReachedExit()
    {
        m_TableState = TableState.NotAssignedTable;
        gameObject.SetActive(false);
        m_CharacterMovement.OnArrivedAtDestination -= ReachedExit;
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
    private IEnumerator GetOrderCoroutine()
    {
        yield return new WaitForSeconds(5);
        m_CoffeeOrder = OrderManager.GetOrder();
        OnOrderChanged.Invoke();
        m_TableState = TableState.Eating;
        yield return null;
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
        Transform exit = TableManager.Instance.GetExit();
        m_CharacterMovement.SetTargetPositionAndRot(exit.position, exit.rotation);
        m_TableState = TableState.FinishedAtTable;
        m_CharacterMovement.OnArrivedAtDestination += ReachedExit;
        OnLeavingTable.Invoke();
    }

    private enum TableState
    {
        NotAssignedTable,
        GettingToTable,
        AtTable,
        Eating,
        FinishedAtTable,
    };
}
