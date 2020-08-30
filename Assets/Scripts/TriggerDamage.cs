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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(this + " collided with : " + other.gameObject);
    //    m_otherObj = other.gameObject;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    m_otherObj = null;
    //}

    public override void DealDamage(Damageable obj)
    {
        //if (!m_otherObj) return;
        obj.OnDamage(Dmg);
        //Debug.Log("Dealing " + (m_DmgMultiplier * Dmg) + " damage to " + obj);
        //obj.OnDamage(Dmg * m_DmgMultiplier);
        Debug.Log("Dealing " + Dmg + " damage to : " + obj + "\nAt hp : " + obj.Health);
    }
}
