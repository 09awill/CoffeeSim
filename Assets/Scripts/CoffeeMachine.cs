using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class CoffeeMachine : Inventory, IInteractable
{
    [SerializeField] private float m_InteractTime = 10f;
    [SerializeField] private Consumable m_Coffee;
    [SerializeField] private Rigidbody m_RB;
    
    [SerializeField] private UnityEvent m_OnInteracting;
    [SerializeField] private UnityEvent m_OnInteractCancelled;
    [SerializeField] private UnityEvent m_OnInteracted;
    private Coroutine m_InteractingCoroutine = null;

    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }


    public void StartInteract()
    {
        if (m_HeldItems.Count < 1) return;
        ConsumableContainer container = m_HeldItems[0] as ConsumableContainer;
        if (!container.IsClearAndClean()) return;
        if (m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_InteractingCoroutine = StartCoroutine(Interacting());
        m_OnInteracting.Invoke();
    }

    public void StopInteract()
    {
        if(m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
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
        if (!m_Coffee) return;
        foreach(var item in m_HeldItems)
        {
            ConsumableContainer container = item as ConsumableContainer;
            Consumable consumable = Instantiate(m_Coffee, container.gameObject.transform.position, container.gameObject.transform.rotation);
            container.AddItem(consumable);
        }
        m_OnInteracted.Invoke();
    }
    public override bool CanHoldObjectType(PickupableObject pObject)
    {
        ConsumableContainer cup = pObject as ConsumableContainer;
        if (cup == null) return false;
        if (m_HeldItems.Count <= 0) return true;
        ConsumableContainer item = m_HeldItems[0] as ConsumableContainer;
        if (item.IsClearAndClean() != cup.IsClearAndClean()) return false;
        return true;
    }
}
