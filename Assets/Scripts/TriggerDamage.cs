using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerDamage : Damage
{
    private Collider m_Collider;

    private GameObject m_otherObj;
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        if (!m_Collider.isTrigger)
            m_Collider.isTrigger = true;
    }

    public override bool DealDamage(Damageable obj)
    {
        if (!obj) return false;
        obj.OnDamage(Dmg);
        return true;
    }
}
