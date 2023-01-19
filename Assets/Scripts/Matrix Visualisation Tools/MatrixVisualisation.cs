using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MatrixVisualisation : MonoBehaviour
{
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private Transform m_EnemyTarget;
    [SerializeField] private float m_LookingPreciseness = 0.5f;
    float m_dotNumber = 0;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 lookingVector = m_TargetTransform.position - transform.position;
        Vector3 targetVector = m_EnemyTarget.position - transform.position;
        bool lookingAt = Vector3.Dot(targetVector.normalized, lookingVector.normalized) >= m_LookingPreciseness;
        
        if(m_dotNumber != Vector3.Dot(targetVector.normalized, lookingVector.normalized)) Debug.Log(Vector3.Dot(targetVector.normalized, lookingVector.normalized));
        m_dotNumber = Vector3.Dot(targetVector.normalized, lookingVector.normalized);
        Handles.color = lookingAt ? Color.green: Color.red;
        Handles.DrawWireArc(transform.position, Vector3.Cross(targetVector, lookingVector), lookingVector, -Vector3.Angle(targetVector, lookingVector), targetVector.magnitude);
        Handles.DrawLine(transform.position, m_EnemyTarget.position + targetVector);

        Handles.DrawLine(transform.position, m_TargetTransform.position + lookingVector);
        Handles.DrawWireDisc(transform.position, lookingVector, 3f);
    }
#endif
}
