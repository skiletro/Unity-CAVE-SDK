using UnityEngine;

/// All the assigned transforms follow this one
/// 2025-04-17 NOTE: Broken as - probably wants disabling 
public class TransformFoller : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsWhichFollow;

    private void Update()
    {
        foreach(GameObject g in objectsWhichFollow)
        {
            g.transform.position = transform.position;
        }


    }
}
