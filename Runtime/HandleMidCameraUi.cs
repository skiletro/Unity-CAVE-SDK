using UnityEngine;

/// vv DEMO BEHAVIOUR vv

public class HandleMidCameraUi : MonoBehaviour
{
	// Simply disables the start menu screen when the button is pressed
    [SerializeField] private GameObject startPanel;

    public void OnStartCaveButtonClick()
    {
        startPanel.SetActive(false);
    }
}
