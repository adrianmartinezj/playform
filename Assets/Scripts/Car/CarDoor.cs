using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : Interactable
{
    private void Start()
    {
        OnPlayerEnter += CarDoor_OnPlayerEnter;
        OnPlayerExit += CarDoor_OnPlayerExit;
        OnUse += CarDoor_OnUse;
    }

    private void CarDoor_OnUse()
    {
        Debug.Log("CarDoor_OnUse");
    }

    private void CarDoor_OnPlayerExit(GameObject player)
    {
        Debug.Log("CarDoor_OnPlayerExit");
    }

    private void CarDoor_OnPlayerEnter(GameObject player)
    {
        Debug.Log("CarDoor_OnPlayerEnter");
    }
}
