using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class GetCoffeeObject : MonoBehaviour, IInteractable, IAcceptItem
{
    [SerializeField] private float m_InteractTime = 10f;
    [SerializeField] private PickupableObject m_Coffee;
    [SerializeField] private Transform m_CoffeeLocation;
    [SerializeField] private Rigidbody m_RB;
    
    [SerializeField] private UnityEvent m_OnInteracting;
    [SerializeField] private UnityEvent m_OnInteractCancelled;
    [SerializeField] private UnityEvent m_OnInteracted;
    private Coroutine m_InteractingCoroutine = null;
    private PickupableObject m_HeldItem;

    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }

    public void StartInteract()
    {
        if (m_HeldItem) return;
        if(m_InteractingCoroutine != null) StopCoroutine(m_InteractingCoroutine);
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
        print("Finished Interaction");
        if (m_Coffee)
        {
            PlaceItem(Instantiate(m_Coffee, m_CoffeeLocation.position, m_CoffeeLocation.rotation));
        }

        m_OnInteracted.Invoke();
    }

    public bool PlaceItem(PickupableObject pObject)
    {
        if (m_HeldItem) return false;
        m_HeldItem = pObject;
        pObject.transform.position = m_CoffeeLocation.position;
        pObject.transform.rotation = m_CoffeeLocation.rotation;
        pObject.AddComponent<FixedJoint>().connectedBody = m_RB;
        return true;
    }

    public PickupableObject PickupItem()
    {
        if (!m_HeldItem) return null;
        Destroy(m_HeldItem.GetComponent<FixedJoint>());
        PickupableObject item = m_HeldItem;
        m_HeldItem = null;
        return item;
    }
}
