using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PropDamageable : Damageable
{
    private Collider m_Collider;
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }

    public override void OnDamage(float dmg)
    {
        base.OnDamage(dmg);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damage damage = collision.gameObject.GetComponent<Damage>();
        if (damage)
        {
            Debug.Log("Interacted with damage source " + collision.gameObject);
            damage.DealDamage(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Damage damage = other.gameObject.GetComponent<Damage>();
        if (damage)
        {
            Debug.Log("Interacted with damage source " + other.gameObject);
            damage.DealDamage(this);
        }
    }
}
