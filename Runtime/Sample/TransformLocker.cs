using UnityEngine;

public class TransformLocker : MonoBehaviour
{
    public bool PositionX = false;
    public bool PositionY = false;
    public bool PositionZ = false;
    public Vector3 positionLocks;


    public bool RotationX = false;
    public bool RotationY = false;
    public bool RotationZ = false;
    public Vector3 RotationLocks;


    public bool ScaleX = false;
    public bool ScaleY = false;
    public bool ScaleZ = false;
    public Vector3 ScaleLocks;


    private void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        if (PositionX) newPosition.x = positionLocks.x;
        if (PositionY) newPosition.y = positionLocks.y;
        if (PositionZ) newPosition.z = positionLocks.z;
        transform.position = newPosition;

        Vector3 newRotation = transform.eulerAngles;
        if (RotationX) newRotation.x = RotationLocks.x;
        if (RotationY) newRotation.y = RotationLocks.y;
        if (RotationZ) newRotation.z = RotationLocks.z;
        transform.eulerAngles = newRotation;

        Vector3 newScale = transform.localScale;
        if (ScaleX) newScale.x = ScaleLocks.x;
        if (ScaleY) newScale.y = ScaleLocks.y;
        if (ScaleZ) newScale.z = ScaleLocks.z;
        transform.localScale = newScale;
    }

}
