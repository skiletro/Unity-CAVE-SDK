using UnityEngine;

/// <summary>
/// Helper utility functions for Cave automatic virtual environment (CAVE) applications
/// </summary>
public static class CAVEUtilities
{

    /// <summary>
    /// Get the camera that contains the mouse position,
    /// Used to determine which camera the mouse is over
    /// 
    /// TODO: Test that this works for the floor
    /// </summary>
    public static Camera GetCameraFromMousePosition(Vector3 mousePos, Camera[] cameras)
    {
        float screenWidth = Screen.width;

        for (int i = 0; i < cameras.Length; i++)
        {
            Camera cam = cameras[i];
            Rect viewportRect = cam.rect;

            // Calculate the screen rect for the camera
            Rect screenRect = new Rect(viewportRect.x * screenWidth, viewportRect.y * Screen.height, viewportRect.width * screenWidth, viewportRect.height * Screen.height);

            if (screenRect.Contains(mousePos))
            {
                return cam;
            }
        }

        // If no camera contains the mouse position, return null
        return null;
    }


    /// <summary>
    /// Raycasts from the mouse position to the world, using the correct CAVE camera and returns the raycast hit.
    /// </summary>
    public static RaycastHit RaycastFromMousePosition(Vector2  mousePos,  Camera[] cameras)
    {
        Camera camera = GetCameraFromMousePosition(mousePos, cameras);
        if (!camera)
        {
            return new RaycastHit();
        }
        Ray ray = camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit;
        }

        return new RaycastHit();
    }
}
