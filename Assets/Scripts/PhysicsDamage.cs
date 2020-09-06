using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PhysicsDamage : Damage
{
    private float m_DmgMultiplier;
    private Rigidbody m_RigidBody;
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        m_DmgMultiplier = m_RigidBody.velocity.magnitude * m_RigidBody.mass;
    }

    public override bool DealDamage(Damageable obj)
    {
        float potentialDmg = m_DmgMultiplier * Dmg;
        // Round damage to two decimals to prevent this from going cray cray
        float temp = Mathf.Floor(potentialDmg) + (Mathf.Floor((potentialDmg - Mathf.Floor(potentialDmg)) * 100) / 100);
        potentialDmg = temp;
        Debug.Log("Dealing " + potentialDmg + " damage to " + obj);

        // Let's not deal with super small damage...
        if (potentialDmg < .01) return false;
        obj.OnDamage(potentialDmg);
        return true;
    }
}
