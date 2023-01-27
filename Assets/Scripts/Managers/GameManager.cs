using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Controls state of game, Manages the spawning and deletion of NPC's and the round difficulty
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (!m_Instance) m_Instance = FindObjectOfType<GameManager>();
            if (!m_Instance) m_Instance = new GameManager();
            return m_Instance;
        }
        set
        {
            if (!m_Instance)
            {
                m_Instance = value;
            }
            else if (m_Instance != value)
            {
                Debug.LogWarning("Too many instances of game manager");
                Destroy(value.gameObject);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] private NPCController m_NPC;
    [SerializeField] private Transform m_NPCSpawnLocation;
    private int m_RoundNumber = 1;
    private NPCController[] m_NPCsInRound;
    private float m_RoundTime = 20;
    private bool m_RoundInProgress = false;
    public void StartRound()
    {
        if (m_NPC == null)
        {
            Debug.LogError("No NPC was assigned in game manager");
            return;
        }
        if (m_RoundInProgress) return;
        int maxNPCs = m_RoundNumber * 3;
        m_RoundTime = m_RoundNumber * 20;
        m_NPCsInRound = new NPCController[maxNPCs];
        StartCoroutine(SpawnNPCs());
        m_RoundInProgress = true;

    }

    private IEnumerator SpawnNPCs()
    {
        float timer = 0;
        int m_NPCCounter = 0;
        while(m_NPCCounter < m_NPCsInRound.Length)
        {
            if(timer > (m_RoundTime/ m_NPCsInRound.Length) * m_NPCCounter)
            {
                m_NPCsInRound[m_NPCCounter] = Instantiate(m_NPC, m_NPCSpawnLocation.position, m_NPCSpawnLocation.rotation);
                m_NPCCounter++;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(EndRoundOnFinished());
    }

    private IEnumerator EndRoundOnFinished()
    {
        bool allFinished = false;
        while (!allFinished)
        {
            allFinished = !m_NPCsInRound.Where(npc => npc.isActiveAndEnabled).Any();
            yield return new WaitForSeconds(1);
        }
        foreach(NPCController controller in m_NPCsInRound)
        {
            Destroy(controller.gameObject);
        }
        m_RoundNumber++;
        m_NPCsInRound = null;
        m_RoundInProgress = false;
    }
}
