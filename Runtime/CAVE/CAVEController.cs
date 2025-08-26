using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;//Enables necessary touch input functions
using UnityEngine.InputSystem.EnhancedTouch;
using Random = UnityEngine.Random;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// General controller for the CAVE system.
/// </summary>
public class CAVEController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the cameras in the CAVE")]
    [SerializeField] private Camera[] cameras;

    [Tooltip("Reference to the CAVE game object")]
    [SerializeField] private GameObject cave;

    [Header("Settings: Movement")]
    [SerializeField] private float movementSpeed = 10f; // Speed of movement
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

    [Header("Settings: touch Actions")]
    [Tooltip("Select the touch action to perform")]
    [SerializeField] private TouchType selectedTouchType = TouchType.None;

    [Tooltip("Allow for either single touch or continuous touch to be enabled.")]
    [SerializeField] private bool allowContinousTouch = true;

    [Tooltip("Tooltip panel for keybind popups.")]
    [SerializeField] private GameObject keybindPanel;

    [Tooltip("Offset to move the CAVE to after the hit point")]
    [SerializeField] private Vector3 teleportOffset = new(0, 1.425f, 0); // Offset to move the CAVE to the hit point

    [Tooltip("The touch actions available to perform")]
    private int keybindsPressedCounter = 0;
    private enum TouchType
    {
        None,                   // No action
        Teleport,               // Teleport the CAVE to the hit point
        SpawnObjectAtPosition,            // Spawn a random object at the hit point (a demo of the raycast)
        ShootProjectile,        // Shoot a projectile from the CAVE at the touch input position
        Touchables,              // Interact with Touchable.cs objects
        Look                    //Drag along CAVE walls to rotate the camera view
    };

    private void Update()
    {
        //HandleTouchActions();
        // Keyboard Input
        CycleTouchInput();
        Quit();
    }

    /*private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }*/

    private void FixedUpdate()
    {
        HandleMovementInput();
        HandleRotationInput();
    }

    #region Touch Actions

    public void HandleTouchActions(InputAction.CallbackContext context)
    { 
        Debug.Log(context.ReadValue<Vector3>());
        // Check for left mouse button click.
        /*bool isTouchAllowed = (allowContinousTouch) ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (!isTouchAllowed)
        {
            return;
        }*/
        RaycastHit raycastHit;
        // Check if raycast is hitting anything, only if input gives a touch position.
        if (context.valueType == typeof(Vector3))
        {
            raycastHit = CAVEUtilities.RaycastFromMousePosition(context.ReadValue<Vector3>(), cameras);
            if (!raycastHit.collider)
            {
                return;
            }
        }
        else
        {
            raycastHit = new RaycastHit();
        }

        // Switch selected TouchType Input
            switch (selectedTouchType)
            {
                case TouchType.Look:
                    //swipe is always between 1 and -1
                    float swipe = context.ReadValue<Vector2>().x;
                    cave.transform.Rotate(Vector3.down, swipe);
                    break; //uses the touch pointer delta to rotate the CAVE camera.
                
                case TouchType.Teleport:
                    MoveCaveToClickPosition(raycastHit);
                    break; //moves CAVE to touch coordinates.
                
                case TouchType.SpawnObjectAtPosition:
                    InstantiateRandomPrimitiveAtClickPosition(raycastHit);
                    break; //Spawn random object at touch coordinates, temporary for demonstration.
                
                case TouchType.Touchables:
                    InteractWithTouchables(raycastHit);
                    break; //prompt a function on any object tagged as a touchable.
                
                case TouchType.ShootProjectile:
                    InstantiateProjectile(raycastHit);
                    break; //Shoot random object towards touch coordinates, temporary for demonstration.
                
                default:
                    Debug.LogWarning("No action selected or action not implemented.");
                    break;
            }
        
    }

    // This is broken, it will only work with one camera.
    private void MoveCaveToClickPosition(RaycastHit hit)
    {
        cave.transform.position = hit.point + teleportOffset; // Move the CAVE to the hit point
    }


    // Spawns a random primitive at the hit point
    private void InstantiateRandomPrimitiveAtClickPosition(RaycastHit hit)
    {
        // List of available primitive types
        PrimitiveType[] primitiveTypes = new PrimitiveType[]
        {
        PrimitiveType.Cube,
        PrimitiveType.Sphere,
        PrimitiveType.Capsule,
        PrimitiveType.Cylinder,
        };

        // Randomly select a primitive type
        PrimitiveType randomType = primitiveTypes[Random.Range(0, primitiveTypes.Length)];

        // Create the primitive and offset
        GameObject primitive = GameObject.CreatePrimitive(randomType);
        float scale = Random.Range(0.2f, 0.5f); // Random scale for the primitive
        primitive.transform.localScale = Vector3.one * scale; // Scale the primitive
        primitive.transform.position = hit.point + hit.normal * scale; // Offset from the surface

        // Add a Rigidbody for physics interactions
        primitive.AddComponent<Rigidbody>();
    }


    private void InteractWithTouchables(RaycastHit hit)
    {
        // Check if the object has a Touchable component.
        Touchable touchable = hit.collider.GetComponent<Touchable>();
        if (touchable)
        {
            touchable.OnTouch(); // Call the OnTouch method on the Touchable component
        }
    }

    // Create a projectile and fire it from the CAVE towards the hit point
    private void InstantiateProjectile(RaycastHit hit)
    {
        // List of available primitive types for the projectile
        PrimitiveType[] primitiveTypes = new PrimitiveType[]
        {
        PrimitiveType.Cube,
        PrimitiveType.Sphere,
        PrimitiveType.Capsule,
        PrimitiveType.Cylinder,
        };

        // Randomly select a primitive type
        PrimitiveType randomType = primitiveTypes[Random.Range(0, primitiveTypes.Length)];

        // Create the projectile GameObject
        GameObject projectile = GameObject.CreatePrimitive(randomType);
        projectile.transform.localScale = Vector3.one * 0.1f; // Scale down the projectile for better visibility
        projectile.transform.position = cave.transform.position; // Start position is the CAVE's position
        Rigidbody rb = projectile.AddComponent<Rigidbody>(); // Add a Rigidbody to the projectile
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Set collision detection mode for better accuracy

        // Add physics material to the projectile
        
        PhysicsMaterial physicsMaterial = new PhysicsMaterial();
        physicsMaterial.bounciness = 0.5f; // Set bounciness
        physicsMaterial.dynamicFriction = 0.5f; // Set dynamic friction
        physicsMaterial.staticFriction = 0.5f; // Set static friction
        projectile.GetComponent<Collider>().material = physicsMaterial; // Assign the physics material to the projectile's collider

        Vector3 direction = (hit.point - cave.transform.position).normalized; // Calculate direction to the hit point
        rb.linearVelocity = direction * 20f; // Set linear velocity of the projectile
    }
    #endregion


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

    private void CycleTouchInput()
    {
        // Cycle through the touch actions
        if (Input.GetKeyDown(KeyCode.G))
        {
            selectedTouchType = (TouchType)(((int)selectedTouchType + 1) % System.Enum.GetValues(typeof(TouchType)).Length);
            string message = $"Selected Touch Type: {selectedTouchType}";
            Debug.Log(message);
            HandleKeybindPopup(message);
        }

        // Cycle between single touch and continous touch option.
        if (Input.GetKeyDown(KeyCode.T))
        {
            allowContinousTouch = !allowContinousTouch;
            string message = $"Allow Continous Touch: {allowContinousTouch}";
            Debug.Log(message);
            HandleKeybindPopup(message);
        }
    }
    #endregion

    private void HandleKeybindPopup(string message)
    {
        keybindPanel.SetActive(true);
        keybindPanel.GetComponentInChildren<TMP_Text>().text = message;
        StartCoroutine(HandleKeybindPopupTimer());
    }

    IEnumerator HandleKeybindPopupTimer()
    {
        keybindsPressedCounter++;
        yield return new WaitForSeconds(2);
        keybindsPressedCounter--;
        Debug.Log(keybindsPressedCounter);
        if (keybindsPressedCounter == 0)
        {
            keybindPanel.SetActive(false);
        }
    }
}
