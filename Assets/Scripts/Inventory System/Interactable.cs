using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Basic interactable script, allows a user to interact and trigger events with a given interact time.
/// </summary>

public class Interactable : MonoBehaviour
{
    [SerializeField] public Action OnInteracting;
    [SerializeField] public Action OnInteractCancelled;
    [SerializeField] public Action OnInteracted;
    [SerializeField] private float m_InteractTime = 5f;
    private Coroutine m_InteractingCoroutine = null;

    public void StartInteract()
    {
        if (!CanStartInteract()) return;
        if (m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        m_InteractingCoroutine = StartCoroutine(Interacting());
        OnInteracting.Invoke();
    }

    public void StopInteract()
    {
        if(m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
        OnInteractCancelled.Invoke();
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

    public void OnInteract()
    {
        OnInteracted.Invoke();
    }
    /// <summary>
    /// Gives option to add restrictions for interaction
    /// </summary>
    /// <returns></returns>
    public bool CanStartInteract()
    {
        return true;
    }
}
