using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Controls total damage done")]
    public float Dmg = 1;

    public virtual void DealDamage(Damageable obj)
    {
        obj.OnDamage(Dmg);
    }
}
