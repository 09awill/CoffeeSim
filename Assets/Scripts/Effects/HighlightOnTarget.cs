using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HighlightOnTarget : MonoBehaviour
{
    [SerializeField] private Inventory m_Inventory;
    [SerializeField] private Renderer m_Renderer;
    [SerializeField] private Color m_EmissiveColour = Color.gray;
    private Material[] m_Materials;
    private Color[] m_InitialColors;
    private void Awake()
    {
        if(!m_Inventory) m_Inventory = GetComponent<Inventory>();
        m_Inventory.OnInventoryTargeted += HighlightObject;
        m_Inventory.OnInventoryUnTargeted += UnHightlightObject;
        if(!m_Renderer) m_Renderer = GetComponent<Renderer>();
        m_Materials = m_Renderer.materials;
        m_InitialColors = m_Materials.Select(c => c.GetColor("_EmissiveColor")).ToArray();
    }
    [ContextMenu("Highlight")]
    public void HighlightObject()
    {
        foreach (Material m in m_Materials)
        {
            m.SetColor("_EmissiveColor", m_EmissiveColour);
        }
    }
    [ContextMenu("UnHighlight")]
    public void UnHightlightObject()
    {
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_EmissiveColor", m_InitialColors[i]);
        }
    }
}
