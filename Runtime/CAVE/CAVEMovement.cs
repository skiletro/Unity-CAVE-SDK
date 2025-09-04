using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

    /// <summary>
    /// General movement controller for the CAVE system.
    /// </summary>

namespace MMUCAVE
{
    public class CAVEMovement : MonoBehaviour
    {
        //Stores the directions passed from the input action callback for use with FixedUpdate.
        Vector3 moveDirection = Vector3.zero;
        Vector2 rotateDirection = Vector2.zero;
        
        [Tooltip("Reference to the CAVE game object")] [SerializeField]
        private GameObject cave; //Reference to the CAVE.
        [Header("Settings: Movement")]
        [Tooltip("Keyboard controlled movement speed")] [SerializeField] private float movementSpeed = 10f;
        [Tooltip("Keyboard controlled rotation speed")] [SerializeField] private float rotationSpeed = 100f;

        public void FixedUpdate()
        {
            //Perform transforms on the cave to apply the directions stored above.
            cave.transform.Translate(moveDirection * (movementSpeed * Time.fixedDeltaTime));
            cave.transform.Rotate(rotateDirection * (rotationSpeed * Time.fixedDeltaTime));
        }

        public void HandleMovementInput(InputAction.CallbackContext context)
        {
            moveDirection = context.ReadValue<Vector3>();//Stores the direction passed from the input system bindings
        }

        public void HandleRotationInput(InputAction.CallbackContext context)
        { 
            rotateDirection = context.ReadValue<Vector2>();//Stores the direction passed from the input system bindings
        }
    }
}
