using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// General controller for the CAVE system.
/// </summary>
public class CAVEController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the cameras in the CAVE")]
    [SerializeField] private Camera[] cameras;

    [Tooltip("Reference to the CAVE game object")]
    [SerializeField] private  GameObject cave;

    [Header("Settings: Movement")]
    [SerializeField] private float movementSpeed = 10f; // Speed of movement
    [SerializeField] private  float rotationSpeed = 100f; // Speed of rotation

    [Header("Settings: touch Actions")]
    [Tooltip("Select the touch action to perform")]
    [SerializeField] private TouchType selectedTouchType = TouchType.None;

    [Tooltip("Allow for either single touch or continuous touch to be enabled.")]
    [SerializeField] private bool allowContinousTouch = true;

    [Tooltip("Tooltip panel for keybind popups.")]
    [SerializeField] private GameObject keybindPanel;

    [Tooltip("Offset to move the CAVE to after the hit point")]
    [SerializeField] private  Vector3 teleportOffset = new(0, 1.425f, 0); // Offset to move the CAVE to the hit point
    
    [Tooltip("The touch actions available to perform")]
    private int keybindsPressedCounter = 0;
    private enum TouchType 
    { 
        None,                   // No action
        Teleport,               // Teleport the CAVE to the hit point
        SpawnSphere,            // Spawn a sphere at the hit point (a demo of the raycast)
        Touchables              // Interact with Touchable.cs objects
    };

    private void Update()
    {
        HandleTouchActions();

        // Keyboard Input
        CycleTouchInput();
        Quit();
    }

    private void FixedUpdate()
    {
        HandleMovementInput();
        HandleRotationInput();
    }

    #region Touch Actions

    private void HandleTouchActions()
    {
        // Check for left mouse button click.
        bool isTouchAllowed = (allowContinousTouch) ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (!isTouchAllowed)
        {
            return;
        }

        // Check if raycast is hitting anything.
        RaycastHit raycastHit = CAVEUtilities.RaycastFromMousePosition(Input.mousePosition, cameras);
        if (!raycastHit.collider)
        {
            return;
        }

        // Switch selected TouchType Input
        switch (selectedTouchType)
        {
            case TouchType.Teleport:
                MoveCaveToClickPosition(raycastHit);
                break;
            case TouchType.SpawnSphere:
                InstantiateSphereAtClickPosition(raycastHit);
                break;
            case TouchType.Touchables:
                InteractWithTouchables(raycastHit);
                break;
        }
    }

    // This is broken, it will only work with one camera.
    private void MoveCaveToClickPosition(RaycastHit hit)
    {
        cave.transform.position = hit.point + teleportOffset; // Move the CAVE to the hit point
    }

    private void InstantiateSphereAtClickPosition(RaycastHit hit)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Create a sphere
        sphere.transform.position = hit.point; // Move the sphere to the hit point
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
