using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Table : MonoBehaviour
{
    [SerializeField] private Chair[] m_Chairs;

    private void Start()
    {
        TableManager.Instance.AddTable(this);
    }

    public Transform GetFreeChair()
    {
        Transform chair = null;
        while (chair == null)
        {
            Chair option = m_Chairs[Random.Range(0, m_Chairs.Length - 1)];
            if (!option.Available) continue;
            chair = option.Transform;
        }

        return chair;
    }
    [Serializable]
    struct Chair
    {
        public bool Available;
        public Transform Transform;
    }
}
