using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    protected List<Collider> m_ActiveColliders = new List<Collider>();
    public List<Collider> ActiveColliders { get { return m_ActiveColliders; } }
    public bool IsInCollision => m_ActiveColliders.Count > 0;
    public event Action ActiveCollidersChanged;
    protected virtual void OnCollisionEnter(Collision collision)
    {
        m_ActiveColliders.Add(collision.collider);
        ActiveCollidersChanged?.Invoke();
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        m_ActiveColliders.Remove(collision.collider);
        ActiveCollidersChanged?.Invoke();
    }
}
