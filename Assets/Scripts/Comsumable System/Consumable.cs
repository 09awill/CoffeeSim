using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Consumable script designed to be as basic as possible and rely on customisable consumable data to make it easily expandable
/// </summary>
public class Consumable : MonoBehaviour
{
    [SerializeField] private SO_Consumable m_ConsumableData;
    private GameObject m_Model;
    private void Awake()
    {
        m_Model = Instantiate(m_ConsumableData.GetInitialModelPrefab(), transform);
    }
    public void Consume()
    {
        Destroy(m_Model);
        m_Model = Instantiate(m_ConsumableData.GetConsumedModelPrefab(), transform);
    }
    public SO_Consumable GetConsumableData()
    {
        return m_ConsumableData;
    }
}
