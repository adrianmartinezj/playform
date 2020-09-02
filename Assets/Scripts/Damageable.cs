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
        Debug.Log("Took " + dmg + " damage");
        Health -= dmg;
        Debug.Log("Health now at " + Health);
        if (Health <= 0)
        {
            Debug.Log(this + " died.");
            Death?.Invoke();
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
