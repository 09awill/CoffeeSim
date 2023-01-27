using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Player interact script allows user to pick up and interact with objects
/// Runs physics overlaps with nearby objects to detect what to interact or pickup when input is pressed.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private bool m_ShowDebug = false;
    [SerializeField] private float m_InteractionPointRadius = 0.5f;
    [SerializeField] private LayerMask m_InteractableMask;
    private readonly Collider[] m_Colliders = new Collider[3];
    private int m_numFound;
    private Collider m_InteractingObject = null;

    public void OnInteract(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            if (m_numFound <= 0) return;
            var interactable = GetClosestInteractable();
            if (interactable == null) return;
            m_InteractingObject = interactable;
            interactable.GetComponent<Interactable>().StartInteract();
        } else if (pContext.canceled)
        {
            if (m_InteractingObject == null) return;
            m_InteractingObject.GetComponent<Interactable>().StopInteract();
            m_InteractingObject = null;
        }
    }

    public void OnStartRound(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            GameManager.Instance.StartRound();
        }
    }

    private void Update()
    {
        m_numFound = Physics.OverlapSphereNonAlloc(transform.position, m_InteractionPointRadius, m_Colliders,
            m_InteractableMask);
        if (!m_InteractingObject) return;
        for (int i = 0; i < m_numFound; i++)
        {
            if(m_InteractingObject == m_Colliders[i])
            {
                return;
            }
        }
        m_InteractingObject.GetComponent<Interactable>().StopInteract();
        m_InteractingObject = null;
    }
    private Collider GetClosestInteractable()
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for (int i = 0; i < m_numFound; i++)
        {
            Collider c = m_Colliders[i];
            float SqDistanceToTarget = (c.gameObject.transform.position - currentPos).sqrMagnitude;
            if (SqDistanceToTarget < minDist)
            {
                Interactable item = c.gameObject.GetComponent<Interactable>();
                if (item == null) continue;
                tMin = c;
                minDist = SqDistanceToTarget;
            }
        }
        return tMin;
    }

    private void OnDrawGizmos()
    {
        if (m_ShowDebug)
        {
            Gizmos.DrawSphere(transform.position, m_InteractionPointRadius);   
        }
    }
}
