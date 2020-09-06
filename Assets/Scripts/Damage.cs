using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damage : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Controls total damage done")]
    public float Dmg = 1;

    /// <summary>
    /// Attempts to damage a damageable object with the parameters defined in the damage type.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>Returns whether or not the damage was dealt.</returns>
    public abstract bool DealDamage(Damageable obj);
}
