using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A script that allows the user to easily interact with objects in the CAVE system.
/// Intended to be attatched to the object / collider that will be interacted with.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Touchable : MonoBehaviour
{
    [Header("Settings: Touchables")]

    [Tooltip("Events which occur when the object registers a touch")]
    [SerializeField] private UnityEvent onTouchEvent;



    public void OnTouch() => onTouchEvent?.Invoke();
}
