using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//Enables necessary touch input functions
using Random = UnityEngine.Random;

namespace MMUCAVE
{
    public class CAVEInputManager : MonoBehaviour
    {

        [Header("References")] [Tooltip("Reference to the cameras in the CAVE")] [SerializeField]
        private Camera[] cameras;

        [Tooltip("Offset to move the CAVE to after the hit point")] [SerializeField]
        private Vector3 teleportOffset = new(0, 1.425f, 0); // Offset to move the CAVE to the hit point

        [Tooltip("The touch actions available to perform")]
        private int keybindsPressedCounter = 0;


        [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

        [Tooltip("Tooltip panel for keybind popups.")] [SerializeField]
        private GameObject keybindPanel;
        

        [Tooltip("Reference to the CAVE game object")] [SerializeField]
        private GameObject cave;
        
        #region Touch Actions

        public void HandleSwipeActions(InputAction.CallbackContext context){
               //swipe is always between 1 and -1
            float swipe = context.ReadValue<Vector2>().x;
            cave.transform.Rotate(Vector3.down, swipe);
            //uses the touch pointer delta to rotate the CAVE camera.

        }
        public void HandleTouchActions(InputAction.CallbackContext context)
        {
            Debug.Log(context.ReadValue<Vector2>());
            RaycastHit raycastHit = CAVEUtilities.RaycastFromMousePosition(context.ReadValue<Vector2>(), cameras);
            if (!raycastHit.collider)
            {
                return;
            }

            // Switch selected TouchType Input
            switch (InputSwitchUtility.selectedTouchType)
            {
                case InputSwitchUtility.TouchType.Teleport:
                    MoveCaveToClickPosition(raycastHit);
                    break; //moves CAVE to touch coordinates.

                case InputSwitchUtility.TouchType.SpawnObject:
                    InstantiateRandomPrimitiveAtClickPosition(raycastHit);
                    break; //Spawn random object at touch coordinates, temporary for demonstration.

                case InputSwitchUtility.TouchType.Touchables:
                    InteractWithTouchables(raycastHit);
                    break; //prompt a function on any object tagged as a touchable.

                case InputSwitchUtility.TouchType.ShootProjectile:
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
            /*PrimitiveType[] primitiveTypes = new PrimitiveType[]
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
            rb.collisionDetectionMode =
                CollisionDetectionMode.ContinuousDynamic; // Set collision detection mode for better accuracy

            // Add physics material to the projectile

            PhysicsMaterial physicsMaterial = new PhysicsMaterial();
            physicsMaterial.bounciness = 0.5f; // Set bounciness
            physicsMaterial.dynamicFriction = 0.5f; // Set dynamic friction
            physicsMaterial.staticFriction = 0.5f; // Set static friction
            projectile.GetComponent<Collider>().material =
                physicsMaterial; // Assign the physics material to the projectile's collider

            Vector3 direction =
                (hit.point - cave.transform.position).normalized; // Calculate direction to the hit point
            rb.linearVelocity = direction * 20f; // Set linear velocity of the projectile
        */
        }

        #endregion
        
        /*private void HandleKeybindPopup(string message)
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
        }*///depricated code
    }
}

