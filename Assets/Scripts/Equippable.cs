using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Slot { rightHand, leftHand, back };

public class Equippable : MonoBehaviour
{
    [SerializeField]
    public Vector3 PickPosition;
    [SerializeField]
    public Vector3 PickRotation;
    [SerializeField]
    public Slot ItemSlot;

    public virtual void OnPickup() { }
}
