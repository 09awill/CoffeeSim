using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private bool m_ShowDebug = false;
    [SerializeField] private float m_InteractionPointRadius = 0.5f;
    [SerializeField] private LayerMask m_InteractableMask;
    [SerializeField] private Rigidbody m_RB;
    private readonly Collider[] m_Colliders = new Collider[3];
    private int m_numFound;
    private Collider m_InteractingObject = null;
    private PickupableObject m_HeldObject = null;

    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }

    public void OnInteract(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            if (m_numFound <= 0) return;
            var interactable = m_Colliders[0].GetComponent<IInteractable>();
            if (interactable == null) return;
            m_InteractingObject = m_Colliders[0];
            interactable.StartInteract();
        } else if (pContext.canceled)
        {
            if (m_InteractingObject == null) return;
            m_InteractingObject.GetComponent<IInteractable>().StopInteract();
            m_InteractingObject = null;
        }
    }

    public void OnPickup(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            Debug.Log("Attempting Pickup");
            if (m_HeldObject)
            {
                if (m_numFound <= 0) return;
                IAcceptItem itemInventory = m_Colliders[0].GetComponent<IAcceptItem>();
                if (itemInventory == null) return;
                bool placed = itemInventory.PlaceItem(m_HeldObject);
                if (placed)
                {
                    m_HeldObject = null;
                }
            }
            else
            {
                if (m_numFound <= 0) return;
                var pickupable = m_Colliders[0].GetComponent<IAcceptItem>()?.PickupItem();
                if (!pickupable) return;
                
                m_HeldObject = pickupable.Pickup();
                m_HeldObject.transform.position = transform.position;
                m_HeldObject.transform.parent = transform;
            }
        }
    }

    private void Update()
    {
        m_numFound = Physics.OverlapSphereNonAlloc(transform.position, m_InteractionPointRadius, m_Colliders,
            m_InteractableMask);
        if (m_InteractingObject && !m_Colliders.Contains(m_InteractingObject))
        {
            m_InteractingObject.GetComponent<IInteractable>().StopInteract();
            m_InteractingObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_ShowDebug)
        {
            Gizmos.DrawSphere(transform.position, m_InteractionPointRadius);   
        }
    }
}
