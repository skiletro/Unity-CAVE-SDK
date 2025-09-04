using UnityEngine;

/// <summary>
/// manually enable all required screens for accurate CAVE projection
/// </summary>
public class DisplayEnabler : MonoBehaviour
{
    #if UNITY_STANDALONE && !UNITY_EDITOR //Only run this script within the build
        [Tooltip("Number of displays connected to the system (-1 for indexing)")][SerializeField] private int displayIndex = 0;
        
        private void OnEnable()
        {
            if (displayIndex >= Display.displays.Length)
            {
                Debug.LogError("Display index out of range");
                return; // If more displays are given than are found, log this as a warning
            }
            
            Display.displays[displayIndex].Activate();// Activate all possible displays
        }
    #endif
}
