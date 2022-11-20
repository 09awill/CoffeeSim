using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody m_RB;
    [SerializeField] private float m_Speed = 10f;
    private Vector2 m_Movement;
    private bool m_Interacting = false;
    
    private void Awake()
    {
        if (!m_RB) m_RB = GetComponent<Rigidbody>();
    }

    public void OnInteracting(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            m_Interacting = true;
        } else if (pContext.canceled)
        {
            m_Interacting = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext pContext)
    {
        m_Movement = pContext.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (m_Interacting)
        {
            m_RB.velocity = Vector3.zero;
            m_RB.angularVelocity = Vector3.zero;
        }
        else
        {
            m_RB.AddForce(new Vector3(m_Movement.x * m_Speed, 0, m_Movement.y * m_Speed));
        }
    }
    
}
