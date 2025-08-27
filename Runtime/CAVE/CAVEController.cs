using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// General controller for the CAVE system.
/// </summary>
namespace MMUCAVE
{
    public class CAVEController : MonoBehaviour
    {

        [Tooltip("Reference to the CAVE game object")] [SerializeField]
        private GameObject cave;

        [Header("Settings: Movement")] [SerializeField]
        private float movementSpeed = 10f; // Speed of movement
        [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

        private void Update()
        {
            Quit();
        }

        private void FixedUpdate()
        {
            HandleMovementInput();
            HandleRotationInput();
        }
        

        #region Keyboard Actions

        void HandleMovementInput()
        {
            // horizontal movement
            if (Input.GetKey(KeyCode.W))
                cave.transform.Translate(Vector3.forward * movementSpeed * Time.fixedDeltaTime); // Move forward
            if (Input.GetKey(KeyCode.S))
                cave.transform.Translate(Vector3.back * movementSpeed * Time.fixedDeltaTime); // Move backward
            if (Input.GetKey(KeyCode.A))
                cave.transform.Translate(Vector3.left * movementSpeed * Time.fixedDeltaTime); // Move left
            if (Input.GetKey(KeyCode.D))
                cave.transform.Translate(Vector3.right * movementSpeed * Time.fixedDeltaTime); // Move right

            // Vertical movement
            if (Input.GetKey(KeyCode.Space))
                cave.transform.Translate(Vector3.up * movementSpeed * Time.fixedDeltaTime); // Move up
            if (Input.GetKey(KeyCode.LeftControl))
                cave.transform.Translate(Vector3.down * movementSpeed * Time.fixedDeltaTime); // Move down

        }

        void HandleRotationInput()
        {
            // Yaw (Y-axis) rotation
            if (Input.GetKey(KeyCode.Q))
                cave.transform.Rotate(Vector3.up, -rotationSpeed * Time.fixedDeltaTime); // Rotate left
            if (Input.GetKey(KeyCode.E))
                cave.transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime); // Rotate right

            // Pitch (X-axis) rotation
            if (Input.GetKey(KeyCode.UpArrow))
                cave.transform.Rotate(Vector3.right, -rotationSpeed * Time.fixedDeltaTime); // Rotate up
            if (Input.GetKey(KeyCode.DownArrow))
                cave.transform.Rotate(Vector3.right, rotationSpeed * Time.fixedDeltaTime); // Rotate down

            // Roll (Z-axis) rotation
            if (Input.GetKey(KeyCode.RightArrow))
                cave.transform.Rotate(Vector3.forward, -rotationSpeed * Time.fixedDeltaTime); // Roll left
            if (Input.GetKey(KeyCode.LeftArrow))
                cave.transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime); // Roll right
        }

        private void Quit()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit(); // Quit the application
        }

        #endregion
    }
}
