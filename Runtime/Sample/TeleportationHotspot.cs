using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using MMUCAVE;
using UnityEngine.Serialization;

/// <summary>
/// Example Interaction Object that teleports the CAVE to its location when tapped.
/// </summary>
public class TeleportationHotspot : InteractionObject
{
    [Tooltip("A reference to the CAVE")]
    [SerializeField] private GameObject cave;

    [Tooltip("The rotation to adjust by when moving to this hotspot")]
    [SerializeField] private Vector3 rotationOffset;

    /// <summary>
    /// Teleports the CAVE to this location
    /// </summary>
    public override void OnTouch()
    {
        cave.transform.position = transform.position;//Moves the CAVE to the position of the hotspot
        cave.transform.rotation = Quaternion.Euler(rotationOffset);//Rotates the CAVE to account for the hotspot's own rotation
    }

}
