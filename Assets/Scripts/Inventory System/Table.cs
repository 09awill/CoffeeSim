using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;
/// <summary>
/// Table script controls the state of the table, which places are full
/// </summary>
public class Table : MonoBehaviour 
{
    [SerializeField] private NPCEatingPlace[] m_Places;

    private void OnValidate()
    {
        if (m_Places != null && m_Places.Length >= 0) return;
        Debug.LogWarning($"[{gameObject.name}] : Table script had no NPCEatingPlaces set, Searching for places in children...");
        m_Places = GetComponentsInChildren<NPCEatingPlace>();
    }
    private void Awake()
    {
        TableManager.Instance.AddTable(this);
    }

    public bool IsFull()
    {
        return m_Places.Where(place => !place.IsAvailable()).Any();
    }

    public Transform SeatNPC(NPCController pNPC)
    {
        if (m_Places.Length <= 0) return null;
        var freePlaces = m_Places.Where(place => place.IsAvailable()).ToList();
        Transform t = freePlaces.Any() ? freePlaces.ElementAt(Random.Range(0, freePlaces.Count)).SeatNPC(pNPC) : null;
        return t; // freePlaces.Any() ? freePlaces.ElementAt(Random.Range(0, freePlaces.Count)).SeatNPC(pNPC) : null;
    }
}
