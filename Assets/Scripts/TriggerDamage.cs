using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerDamage : Damage
{
    private Collider m_Collider;
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        if (!m_Collider.isTrigger)
            m_Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this + " collided with : " + other.gameObject);
    }

    public override void DealDamage(Damageable obj)
    {
        //Debug.Log("Dealing " + (m_DmgMultiplier * Dmg) + " damage to " + obj);
        //obj.OnDamage(Dmg * m_DmgMultiplier);
    }
}
