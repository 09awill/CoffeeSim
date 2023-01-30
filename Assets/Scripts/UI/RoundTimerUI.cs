using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField] private Image m_UIFill;
    private Coroutine m_TimerCoroutine;
    private void Awake()
    {
        GameManager.Instance.OnStartRoundTimer += StartTimer;
    }
    private void StartTimer(float pTime)
    {
        if (m_TimerCoroutine != null) StopCoroutine(m_TimerCoroutine);
        m_TimerCoroutine = StartCoroutine(TimerCoroutine(pTime));
    }
    private IEnumerator TimerCoroutine(float pTime)
    {
        float m_RoundTimer = 0;
        while (m_RoundTimer <= pTime)
        {
            UpdateTimerText(m_RoundTimer, pTime);
            m_RoundTimer += Time.deltaTime;
            yield return null;
        }
        UpdateTimerText(m_RoundTimer, pTime);
    }
    private void UpdateTimerText(float pCurrentTime, float pRoundTime)
    {
        int mins = (int)Mathf.Floor(pCurrentTime / 60);
        int secs = (int)Mathf.Floor(pCurrentTime % 60);
        m_TextMeshProUGUI.text = string.Format("{0:00}:{1:00}", mins, secs);
        m_UIFill.fillAmount = Mathf.InverseLerp(0, pRoundTime, pCurrentTime);
    }
}
