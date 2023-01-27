using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NPCOrderImageUI : MonoBehaviour
{
    [SerializeField] private GameObject OrderUI;
    [SerializeField] private Image m_OrderImage;
    [SerializeField] private NPCController m_NPCController;
    private Camera m_Camera;
    private void Awake()
    {
        if (!m_OrderImage)
        {
            enabled = false;
            return;
        }
        m_Camera = Camera.main;
        m_NPCController.OnOrderChanged += CheckOrderState;
        m_NPCController.OnEating += Hide;
        Hide();
    }

    private void CheckOrderState()
    {
        List<Consumable> list = m_NPCController.GetOrder();
        if (list == null)
        {
            Hide();
            return;
        }
        m_OrderImage.sprite = list.First().GetConsumableData().GetConsumableImage();
        Show();
    }
    private void Show()
    {
        OrderUI.SetActive(true);
    }
    private void Hide()
    {
        OrderUI.SetActive(false);
    }

    private void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
    }
}
