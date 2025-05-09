using UnityEngine;

public class HandleMidCameraUi : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartCaveButtonClick()
    {
        startPanel.SetActive(false);
    }
}
