using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sink : Inventory, IInteractable
{
    [SerializeField] private UnityEvent m_OnInteracting;
    [SerializeField] private UnityEvent m_OnInteractCancelled;
    [SerializeField] private UnityEvent m_OnInteracted;
    [SerializeField] private float m_InteractTime = 5f;
    private Coroutine m_InteractingCoroutine = null;
    public void StartInteract()
    {
        if (m_HeldItems.Count < 1) return;
        ConsumableContainer container = m_HeldItems[0] as ConsumableContainer;
        if (container.IsClearAndClean()) return;
        if (m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_InteractingCoroutine = StartCoroutine(Interacting());
        m_OnInteracting.Invoke();
    }
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer plate = pObject as ConsumableContainer;
        if (plate == null) return false;
        if (m_HeldItems.Count <= 0) return true;
        ConsumableContainer item  = m_HeldItems[0] as ConsumableContainer;
        if (item.IsClearAndClean() != plate.IsClearAndClean()) return false;
        return true;
    }

    public void StopInteract()
    {
        if (m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_OnInteractCancelled.Invoke();
    }

    private IEnumerator Interacting()
    {
        float m_InteractingTime = 0;
        while (m_InteractingTime < m_InteractTime)
        {
            m_InteractingTime += Time.deltaTime;
            yield return null;
        }
        OnInteract();
        yield return null;
    }

    public void OnInteract()
    {
        foreach (var item in m_HeldItems)       
        {
            ConsumableContainer container = item as ConsumableContainer;
            container.Clean();
        }
        m_OnInteracted.Invoke();
    }
}
