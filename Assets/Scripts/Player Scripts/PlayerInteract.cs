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
    [SerializeField] private Rigidbody m_RB;
    [SerializeField] private Transform m_HeldItemLocation;
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
            var interactable = GetClosestInteractable();
            if (interactable == null) return;
            m_InteractingObject = interactable;
            interactable.GetComponent<IInteractable>().StartInteract();
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
            if (m_HeldObject)
            {
                if (m_numFound <= 0) return;
                Inventory itemInventory = GetClosestInventory();
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
                var pickupable = GetClosestInventory()?.PickupItem();
                if (!pickupable) return;
                m_HeldObject = pickupable;
                m_HeldObject.transform.position = m_HeldItemLocation.position;
                m_HeldObject.transform.parent = transform;
            }
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
        if (m_InteractingObject && !m_Colliders.Contains(m_InteractingObject))
        {
            m_InteractingObject.GetComponent<IInteractable>().StopInteract();
            m_InteractingObject = null;
        }
    }

    private Inventory GetClosestInventory()
    {
        Inventory tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Collider c in m_Colliders)
        {
            float dist = Vector3.Distance(c.gameObject.transform.position, currentPos);
            if (dist < minDist)
            {
                Inventory item = c.gameObject.GetComponent<Inventory>();
                if (item == null) continue;
                tMin = item;
                minDist = dist;
            }
        }
        return tMin;
    }
    private Collider GetClosestInteractable()
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Collider c in m_Colliders)
        {
            float dist = Vector3.Distance(c.gameObject.transform.position, currentPos);
            if (dist < minDist)
            {
                IInteractable item = c.gameObject.GetComponent<IInteractable>();
                if (item == null) continue;
                tMin = c;
                minDist = dist;
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
