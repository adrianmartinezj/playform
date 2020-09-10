using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerMode
{
    OneHand,
    TwoHand,
    Magic,
    Unarmed
};

public abstract class Ability : MonoBehaviour
{
    public string Name;
    public EPlayerMode Mode;

    public abstract void Equipped(PlayerController player);
    public abstract void BeginUse(PlayerController player);
    public abstract void EndUse(PlayerController player);
}
