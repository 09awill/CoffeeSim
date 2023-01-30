using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Player interact script allows user to pick up and interact with objects
/// Runs physics overlaps with nearby objects to detect what to interact or pickup when input is pressed.
/// </summary>
[RequireComponent(typeof(PlayerInteractableDetector))]
public class PlayerInteract : MonoBehaviour
{
    private PlayerInteractableDetector m_Detector;
    private Interactable m_InteractingObject = null;
    private void Awake()
    {
        m_Detector = GetComponent<PlayerInteractableDetector>();
    }
    public void OnInteract(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            var interactable = GetClosestInteractable();
            if (interactable == null) return;
            m_InteractingObject = interactable;
            interactable.StartInteract();
        } else if (pContext.canceled)
        {
            if (m_InteractingObject == null) return;
            m_InteractingObject.StopInteract();
            m_InteractingObject = null;
        }
    }

    public void OnStartRound(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            GameManager.Instance.StartRound();
        }
    }

    private void Update()
    {
        if (!m_InteractingObject) return;
        GameObject[] interactables = m_Detector.getInteractables();
        if (interactables != null && interactables.Contains(m_InteractingObject.gameObject))
        {
            return;
        }
        m_InteractingObject.StopInteract();
        m_InteractingObject = null;
    }
    private Interactable GetClosestInteractable()
    {
        Interactable tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        GameObject[] interactables = m_Detector.getInteractables();
        if (interactables == null) return null;
        foreach (GameObject interactable in interactables)
        {
            float SqDistanceToTarget = (interactable.transform.position - currentPos).sqrMagnitude;
            if (SqDistanceToTarget < minDist)
            {
                Interactable item = interactable.GetComponent<Interactable>();
                if (item == null) continue;
                tMin = item;
                minDist = SqDistanceToTarget;
            }
        }
        return tMin.GetComponent<Interactable>();
    }

}
