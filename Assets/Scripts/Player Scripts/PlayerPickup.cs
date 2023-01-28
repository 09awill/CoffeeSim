using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInteractableDetector))]
public class PlayerPickup : MonoBehaviour
{
    private PlayerInteractableDetector m_Detector;
    [SerializeField] private Transform m_HeldItemLocation;
    private PickupableObject m_HeldObject = null;
    private Inventory m_ClosestInventory = null;

    private void Awake()
    {
        m_Detector = GetComponent<PlayerInteractableDetector>();
    }
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
                    m_ClosestInventory?.SetAsTarget(m_HeldObject);
                }
            }
            else
            {
                var pickupable = GetClosestInventory()?.PickupItem();
                if (!pickupable) return;
                m_HeldObject = pickupable;
                m_HeldObject.transform.position = m_HeldItemLocation.position;
                m_HeldObject.transform.parent = transform;
                m_ClosestInventory?.SetAsTarget(m_HeldObject);
            }
        }
    }
    private void Update()
    {
        Inventory prevInv = m_ClosestInventory;
        m_ClosestInventory = GetClosestInventory();
        if (prevInv != m_ClosestInventory && prevInv != null) 
        {
            prevInv.RemoveAsTarget();
        }
        m_ClosestInventory?.SetAsTarget(m_HeldObject);
        prevInv = m_ClosestInventory;
    }

    private Inventory GetClosestInventory()
    {
        Inventory tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        GameObject[] inventories = m_Detector.getInteractables();
        if (inventories == null) return null;
        foreach(GameObject inventory in inventories) 
        { 
            float sqDistanceToTarget = (inventory.transform.position - currentPos).sqrMagnitude;
            if (sqDistanceToTarget < minDist)
            {
                Inventory item = inventory.gameObject.GetComponent<Inventory>();
                if (item == null) continue;
                tMin = item;
                minDist = sqDistanceToTarget;
            }
        }
        return tMin;
    }
}
