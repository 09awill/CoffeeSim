using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractableDetector : MonoBehaviour
{
    [SerializeField] private Transform m_InteractionCenter;
    [SerializeField] private float m_InteractableRadius = 2;
    [SerializeField] private LayerMask m_InteractableMask;
    [SerializeField] private bool m_ShowDebug = false;
    private readonly Collider[] m_Colliders = new Collider[3];
    private int m_numFound;
    private void Awake()
    {
        if (!m_InteractionCenter) m_InteractionCenter = transform;
    }

    private void Update()
    {
        m_numFound = Physics.OverlapSphereNonAlloc(m_InteractionCenter.position, m_InteractableRadius, m_Colliders,
    m_InteractableMask);
    }
    public GameObject[] getInteractables()
    {
        if (m_numFound == 0) return null;

        Collider[] subArray = new Collider[m_numFound];
        Array.Copy(m_Colliders, 0, subArray, 0, m_numFound);
        return subArray.Select(c => c.gameObject).ToArray();
    }

    private void OnDrawGizmos()
    {
        if (m_ShowDebug)
        {
            Gizmos.DrawSphere(transform.position, m_InteractableRadius);
        }
    }
}
