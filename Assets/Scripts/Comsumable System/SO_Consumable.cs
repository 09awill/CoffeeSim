using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic consumable data
/// </summary>
[CreateAssetMenu(menuName ="Consumables/Create new consumable")]
public class SO_Consumable : ScriptableObject
{
    [SerializeField] private GameObject m_InitialModelPrefab;
    [SerializeField] private GameObject m_ConsumedModelPrefab;
    [SerializeField] private Sprite m_ConsumableImage;
    public GameObject GetInitialModelPrefab()
    {
        return m_InitialModelPrefab;
    }
    public GameObject GetConsumedModelPrefab()
    {
        return m_ConsumedModelPrefab;
    }
    public Sprite GetConsumableImage()
    {
        return m_ConsumableImage;
    }
}
