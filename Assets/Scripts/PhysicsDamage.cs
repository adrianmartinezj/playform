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

    public override void DealDamage(Damageable obj)
    {
        Debug.Log("Dealing " + (m_DmgMultiplier * Dmg) + " damage to " + obj);
        obj.OnDamage(Dmg * m_DmgMultiplier);
    }
}
