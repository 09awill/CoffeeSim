using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Cleaner script is an interactable which cleans items held in an inventory.
/// </summary>
[RequireComponent(typeof(Inventory)), RequireComponent(typeof(Interactable))]
public class CleanOnInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStartClean;
    [SerializeField] private UnityEvent OnStopClean;
    private Inventory m_Inventory;
    private bool m_Interacting = false;

    private void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
        Interactable interactable = GetComponent<Interactable>();
        interactable.OnInteracting += () => StartInteract();
        interactable.OnInteractCancelled += () => CancelInteract();
        interactable.OnInteracted += () => OnInteract();
    }
    public void StartInteract()
    {
        List<ConsumableContainer> consumableContainers = m_Inventory.GetListOfItems().Select(cc => (ConsumableContainer)cc).ToList();
        if (!consumableContainers.Any()) return;
        if (!consumableContainers.Where(cc => !cc.IsClearAndClean()).ToList().Any()) return;
        m_Interacting = true;
        OnStartClean.Invoke();
    }
    public void CancelInteract()
    {
        m_Interacting = false;
        OnStopClean.Invoke();
    }
    public void OnInteract()
    {
        List<PickupableObject> items = m_Inventory.GetListOfItems();
        foreach (var item in items)
        {
            ConsumableContainer container = item as ConsumableContainer;
            container.Clean();
        }
        OnStopClean.Invoke();
    }

}
