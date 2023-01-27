using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Basic Movement Script to allow the user to get around and test interactions - need to update to a better feeling system
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_TurnSmoothTime = 0.1f;
    private Vector2 m_Movement;
    private bool m_Interacting = false;
    private float m_TurnSmoothVelocity;

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
            m_Movement = Vector2.zero;
        }
        if (m_Movement.magnitude < 0.1f) return;

        float targetAngle = Mathf.Atan2(m_Movement.x, m_Movement.y) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_TurnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        m_CharacterController.Move(moveDir * m_Speed * Time.deltaTime);

    }
    
}
