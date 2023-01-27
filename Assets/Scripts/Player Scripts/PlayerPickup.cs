using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float m_PickupableRadius = 0.5f;
    [SerializeField] private Transform m_HeldItemLocation;
    [SerializeField] private LayerMask m_PickupableMask;
    private readonly Collider[] m_Colliders = new Collider[3];
    private PickupableObject m_HeldObject = null;


    public void OnPickup(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            if (m_HeldObject)
            {
                Inventory itemInventory = GetClosestInventory();
                if (itemInventory == null) return;
                bool placed = itemInventory.TryPlaceItem(m_HeldObject);
                if (placed)
                {
                    m_HeldObject = null;
                }
            }
            else
            {
                var pickupable = GetClosestInventory()?.PickupItem();
                if (!pickupable) return;
                m_HeldObject = pickupable;
                m_HeldObject.transform.position = m_HeldItemLocation.position;
                m_HeldObject.transform.parent = transform;
            }
        }
    }

    private Inventory GetClosestInventory()
    {
        Inventory tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        var numFound = Physics.OverlapSphereNonAlloc(transform.position, m_PickupableRadius, m_Colliders, m_PickupableMask);
        for (int i = 0; i < numFound; i++)
        {
            Collider c = m_Colliders[i];
            float sqDistanceToTarget = (c.gameObject.transform.position - currentPos).sqrMagnitude;
            if (sqDistanceToTarget < minDist)
            {
                Inventory item = c.gameObject.GetComponent<Inventory>();
                if (item == null) continue;
                tMin = item;
                minDist = sqDistanceToTarget;
            }
        }
        return tMin;
    }
}
