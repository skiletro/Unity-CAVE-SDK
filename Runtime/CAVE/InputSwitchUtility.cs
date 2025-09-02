using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InputSwitchUtility : MonoBehaviour
{
    public enum TouchTypes
    {
        None, // No action
        Teleport, // Teleport the CAVE to the hit point
        SpawnObject, // Spawn a random object at the hit point (a demo of the raycast)
        ShootProjectile, // Shoot a projectile from the CAVE at the touch input position
        Touchables, // Interact with Touchable.cs objects
        Look //Drag along CAVE walls to rotate the camera view
    };

    [Header("Settings: touch Actions")] [Tooltip("Select the touch action to perform")] [SerializeField]
    public static TouchTypes selectedTouchType = TouchTypes.Touchables;

    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private GameObject label;


    public void CycleTouchInput(string touchType)
    {
        selectedTouchType = Enum.TryParse<TouchTypes>(touchType, true, out var result) ? result : TouchTypes.None; //check if passed string matches a touch type
        string message = $"Selected Touch Type: {selectedTouchType}"; //change UI label to display current touch type
        label.GetComponent<TMP_Text>().text = message;
        if (result == TouchTypes.Look) 
        {
            EnableSwipeActionMap();// if touch type is look, enable swiping
        }
        else if (playerInput.actions.FindActionMap("Swipe").enabled) 
        {
            DisableSwipeActionMap();// if touch type is not look but swiping is enabled, disable it
        }
    }

    private void EnableSwipeActionMap()
    {
        playerInput.actions.FindActionMap("Swipe").Enable();
        playerInput.actions.FindActionMap("Touch").Disable();
    }
    private void DisableSwipeActionMap()
    {
        playerInput.actions.FindActionMap("Swipe").Disable(); 
        playerInput.actions.FindActionMap("Touch").Enable();
    }
}
