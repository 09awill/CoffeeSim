using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    private void Awake()
    {
        if (!m_NavMeshAgent) m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_TargetTransform.position = TableManager.Instance.GetChair().position;
    }

    private void Update()
    {
        m_NavMeshAgent.destination = m_TargetTransform.position;
    }
    
}
