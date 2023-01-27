using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class ParticlesOnInteract : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ParticleSystem;
    private void Awake()
    {
        if (m_ParticleSystem == null && !TryGetComponent(out m_ParticleSystem))
        {
            enabled = false;
            return;
        }
        Interactable interactable = GetComponent<Interactable>();
        interactable.OnInteracting += () => StartParticles();
        interactable.OnInteractCancelled += () => StopParticles();
        interactable.OnInteracted += () => StopParticles();
    }
    private void StartParticles()
    {
        m_ParticleSystem.Play();
    }

    private void StopParticles()
    {
        m_ParticleSystem.Stop();
    }
}
