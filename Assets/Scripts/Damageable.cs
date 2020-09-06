using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Controls the total HP for a damageable")]
    public float Health = 10;
    // Event called when the Damageable "dies"
    public event Action Death;

    public virtual void OnDamage(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            Death?.Invoke();
            OnDeath();
        }
    }

    public abstract void OnDeath();
}
