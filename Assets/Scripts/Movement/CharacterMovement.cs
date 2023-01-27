using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    public Action OnArrivedAtDestination;
    [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private bool m_DrawDebug = false;
    private bool m_HasNewTarget;
    private void Awake()
    {
        if (!m_NavMeshAgent) m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_TargetTransform = new GameObject($"[{gameObject.name}] Nav Agent Target").transform;
    }
    public Action SetTargetPositionAndRot(Vector3 pPos, Quaternion pRot)
    {
        m_TargetTransform.SetPositionAndRotation(pPos, pRot);
        m_HasNewTarget = true;
        return OnArrivedAtDestination;
    }
    private void Update()
    {
        if (m_NavMeshAgent.transform.position != m_TargetTransform.position) m_NavMeshAgent.destination = m_TargetTransform.position;
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
                    if (m_HasNewTarget)
                    {
                        m_HasNewTarget = false;
                        OnArrivedAtDestination.Invoke();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (m_DrawDebug) Gizmos.DrawLine(transform.position, transform.position + m_TargetTransform.forward);
    }
    private void OnDestroy()
    {
        if (m_TargetTransform) Destroy(m_TargetTransform.gameObject);
    }

}
