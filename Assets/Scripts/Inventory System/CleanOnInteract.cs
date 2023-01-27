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
        var consumableContainers = m_Inventory.GetListOfItems().Select(item => item.GetComponent<ConsumableContainer>()).Where(item => item != null).ToList();
        if (!consumableContainers.Any()) return;
        if (!consumableContainers.Where(cc => !cc.IsClearAndClean()).ToList().Any()) return;
        OnStartClean.Invoke();
    }
    public void CancelInteract()
    {
        OnStopClean.Invoke();
    }
    public void OnInteract()
    {
        var consumableContainers = m_Inventory.GetListOfItems().Select(item => item.GetComponent<ConsumableContainer>()).Where(item => item != null).ToList();
        foreach (var consumableContainer in consumableContainers)
        {
            consumableContainer.Clean();
        }
        OnStopClean.Invoke();
    }

}
