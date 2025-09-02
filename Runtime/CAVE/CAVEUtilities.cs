using UnityEngine;

/// <summary>
/// Helper utility functions for Cave automatic virtual environment (CAVE) applications
/// </summary>
public static class CAVEUtilities
{
    public static enum TouchTypes //Types of touch input that can be handled
    {
        None, // No action
        Teleport, // Teleport the CAVE to the hit point
        SpawnObject, // Spawn a random object at the hit point (a demo of the raycast)
        ShootProjectile, // Shoot a projectile from the CAVE at the touch input position
        Touchables, // Interact with Touchable.cs objects
        Look //Drag along CAVE walls to rotate the camera view
    };

    public static Camera GetCameraFromScreenPosition(Vector3 pos, Camera[] cameras)
    {	// Get the camera that contains the mouse position,
       	// Used to determine which camera the mouse is over
        float screenWidth = Screen.width;

        for (int i = 0; i < cameras.Length; i++)
        {
			// Check each camera on the CAVE
            Camera cam = cameras[i];
            Rect viewportRect = cam.rect;

            // Calculate the screen rect for the camera
            Rect screenRect = new Rect(viewportRect.x * screenWidth, viewportRect.y * Screen.height, viewportRect.width * screenWidth, viewportRect.height * Screen.height);

            if (screenRect.Contains(pos))
            {
                return cam;// If the position passed in is within that camera, return a reference to it
            }
        }

        // If no camera contains the mouse position, return null
        return null;
    }


    /// <summary>
    /// Raycasts from the given position to the corresponding world point, uses the correct CAVE camera for accurate results and returns the raycast hit
    /// </summary>
    public static RaycastHit RaycastFromScreenPosition(Vector2  pos,  Camera[] cameras)
    {
        Camera camera = GetCameraFromScreenPosition(pos, cameras);
        if (!camera)
        {
            return new RaycastHit();
        }
        Ray ray = camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit;
        }

        return new RaycastHit();
    }
}
