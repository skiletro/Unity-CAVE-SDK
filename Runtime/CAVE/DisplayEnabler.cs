using UnityEngine;

public class DisplayEnabler : MonoBehaviour
{
    [SerializeField] private int displayIndex = 0;
    
    private void OnEnable()
    {
        if (displayIndex >= Display.displays.Length)
        {
            Debug.LogError("Display index out of range");
            return;
        }
        
        Display.displays[displayIndex].Activate();
    }
}
