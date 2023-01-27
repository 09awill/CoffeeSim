using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Script to add consumable to container on interact
/// </summary>
[RequireComponent(typeof(Inventory)), RequireComponent(typeof(Interactable))]
public class AddConsumableOnInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStartInteract;
    [SerializeField] private UnityEvent OnStopInteract;
    [SerializeField] private Consumable m_Consumable;
    private Inventory m_Inventory;
    bool m_Interacting = false;

    private void Awake()
    {
        if (!m_Consumable)
        {
            Debug.LogWarning($"[{gameObject.name}] : AddConsumableOnInteractScript was not provided a consumable, Disabling...");
            enabled = false;
            return;
        }
        m_Inventory = GetComponent<Inventory>();
        Interactable interactable = GetComponent<Interactable>();
        interactable.OnInteracting += () => StartInteract();
        interactable.OnInteractCancelled += () => CancelInteract();
        interactable.OnInteracted += () => OnInteract();
    }
    public void StartInteract()
    {
        var consumableContainers = m_Inventory.GetListOfItems().Select(item => item.GetComponent<ConsumableContainer>()).Where(item => item != null).ToList();
        if (!consumableContainers.Where(cc => !cc.GetConsumableData().Contains(m_Consumable.GetConsumableData())).ToList().Any()) return;
        m_Interacting = true;
        OnStartInteract.Invoke();
    }
    public void CancelInteract()
    {
        m_Interacting = false;
        OnStopInteract.Invoke();
    }
    public void OnInteract()
    {
        if (!m_Interacting) return;
        var consumableContainers = m_Inventory.GetListOfItems().Select(item => item.GetComponent<ConsumableContainer>()).Where(item => !item.GetConsumableData().Contains(m_Consumable.GetConsumableData())).ToList();
        foreach (var consumableContainer in consumableContainers)
        {
            consumableContainer.AddItem(Instantiate(m_Consumable, consumableContainer.gameObject.transform.position, consumableContainer.gameObject.transform.rotation));
        }
        OnStopInteract.Invoke();
    }
}
