using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {

    }

    public override void Equipped(PlayerController player)
    {
        
    }

    public override void BeginUse(PlayerController player)
    {
        player.Animator.SetLayerWeight(1, 1);
        player.Animator.SetBool("IsSwinging", true);
    }

    public override void EndUse(PlayerController player)
    {
        player.Animator.SetBool("IsSwinging", false);
    }
}
