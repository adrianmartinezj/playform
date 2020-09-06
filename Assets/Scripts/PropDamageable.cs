using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

[RequireComponent(typeof(Collider))]
public class PropDamageable : Damageable
{
    private Collider m_Collider;
    private Vector3 m_ContactPoint;
    private const float DEFORM_MULTIPLIER = 1f;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }

    public override void OnDamage(float dmg)
    {
        base.OnDamage(dmg);
        ImpactMorphMesh(dmg * DEFORM_MULTIPLIER, m_ContactPoint);
    }

    /// <summary>
    /// Handles collision based events for damage types like PhysicsDamage and AnimationDamage.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Damage damage = collision.gameObject.GetComponent<Damage>();
        EvaluateCollisionDamage(damage, collision);
    }

    /// <summary>
    /// Handles trigger based events for damage types like TriggerDamage.
    /// </summary>
    /// <param name="other"></param>
    //private void OnTriggerEnter(Collider other)
    //{
    //    Damage damage = other.gameObject.GetComponent<Damage>();
    //    Debug.Log("Interacted with damage source " + other.gameObject);
    //    EvaluateTriggerDamage(damage);
    //}

    private void EvaluateCollisionDamage(Damage dmg, Collision collision)
    {
        if (dmg)
        {
            m_ContactPoint = collision.GetContact(0).point;
            dmg.DealDamage(this);
        }
    }

    private void ImpactMorphMesh(float magnitude, Vector3 contactPoint)
    {
        Dentable dentable = GetComponent<Dentable>();
        if (!dentable) return;

        dentable.DentAtPosition(contactPoint);

        //GameObject newDeformer = new GameObject("Bend");
        //newDeformer.transform.parent = this.transform;
        //BendDeformer bendDeformer = newDeformer.AddComponent<BendDeformer>();

        //newDeformer.transform.position = contactPoint;
        //newDeformer.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

        //bendDeformer.Angle = -5.0f;
        //bendDeformer.Factor = magnitude;

        //deformable.AddDeformer(bendDeformer);
    }

    public override void OnDeath()
    {
        // Cleanup the deformers
        //Deformable deformable = GetComponent<Deformable>();
        //foreach (var deform in deformable.DeformerElements)
        //{
        //    Destroy(deform.Component.gameObject);
        //}
        Debug.Log("On death!");
        Destroy(gameObject);
    }
}
