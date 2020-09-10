﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    // ---- Public ----
    public const string PLAYER_TAG = "Player";
    // ---- Protected ----
    protected delegate void PlayerEvent(GameObject player);
    protected event PlayerEvent OnPlayerEnter;
    protected event PlayerEvent OnPlayerExit;
    protected event Action OnUse;
    // ---- Private ----
    private bool m_InRange = false;
    private Collider m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        if (!m_Collider.isTrigger)
            m_Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            OnPlayerEnter?.Invoke(other.gameObject);
            m_InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            OnPlayerExit?.Invoke(other.gameObject);
            m_InRange = false;
        }
    }

    public void Use()
    {
        OnUse?.Invoke();
    }
}
