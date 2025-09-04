using UnityEngine;

///<summary> vv DEMO BEHAVIOUR vv <br> Disables start panel when button pressed. </br> </summary>

public class HandleMidCameraUi : MonoBehaviour
{
	// Simply disables the start menu screen when the button is pressed
    [SerializeField] private GameObject startPanel;

    public void OnStartCaveButtonClick()
    {
        startPanel.SetActive(false);//disable spart panel
    }
}
