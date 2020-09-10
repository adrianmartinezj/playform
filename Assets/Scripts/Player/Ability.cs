using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerMode
{
    OneHand,
    TwoHand,
    Magic,
    Unarmed,
    Misc
};

public abstract class Ability : MonoBehaviour
{
    public string Name;
    public EPlayerMode Mode;
    public Sprite Icon;

    public virtual void Equipped(PlayerController player)
    {
        HUDController.Instance.UpdateIcon(this.Icon);
    }
    public virtual void Unequipped()
    {
        HUDController.Instance.ClearIcon();
    }
    public virtual void BeginUse(PlayerController player) { }
    public virtual void EndUse(PlayerController player) { }
}
