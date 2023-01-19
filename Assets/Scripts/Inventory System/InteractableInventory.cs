using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Need to adjust this script to be more component based. Currently uses basically the same code as the sink so doesn't need to be two scripts
/// </summary>

public class InteractableInventory : Inventory, IInteractable
{
    [SerializeField] private float m_InteractTime = 10f;
    [SerializeField] private UnityEvent m_OnInteracting;
    [SerializeField] private UnityEvent m_OnInteractCancelled;
    [SerializeField] private UnityEvent m_OnInteracted;
    private Coroutine m_InteractingCoroutine = null;

    public void StartInteract()
    {
        if (!CanStartInteract()) return;
        if (m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_InteractingCoroutine = StartCoroutine(Interacting());
        m_OnInteracting.Invoke();
    }

    public void StopInteract()
    {
        if(m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_OnInteractCancelled.Invoke();
    }

    public IEnumerator Interacting()
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

    public virtual void OnInteract()
    {
        m_OnInteracted.Invoke();
    }
    public virtual bool CanStartInteract()
    {
        return true;
    }
}
