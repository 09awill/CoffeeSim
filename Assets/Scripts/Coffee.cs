using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coffee : Consumable
{
    [SerializeField] GameObject m_InitialModel;
    [SerializeField] GameObject m_ConsumedModel;
    public override void Consume()
    {
        m_InitialModel.SetActive(false);
        m_ConsumedModel.SetActive(true);
    }
}
