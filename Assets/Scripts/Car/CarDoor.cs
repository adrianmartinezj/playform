using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : Interactable
{
    private void Start()
    {
        OnPlayerEnter += CarDoor_OnPlayerEnter;
        OnPlayerExit += CarDoor_OnPlayerExit;
    }

    private void CarDoor_OnPlayerExit(GameObject player)
    {
        Debug.Log("CarDoor_OnPlayerExit");
        player.GetComponent<PlayerController>().UpdateAbility(null);
    }

    private void CarDoor_OnPlayerEnter(GameObject player)
    {
        Debug.Log("CarDoor_OnPlayerEnter");
        player.GetComponent<PlayerController>().UpdateAbility(this);
    }

    public override void BeginUse(PlayerController player)
    {
        if (!InUse)
        {
            Debug.Log("Opening car door...");
            StartCoroutine(WaitForCarDoor());
        }
    }

    IEnumerator WaitForCarDoor()
    {
        InUse = true;
        yield return new WaitForSeconds(1.5f);
        InUse = false;
        Debug.Log("Car door has been opened.");
    }
}
