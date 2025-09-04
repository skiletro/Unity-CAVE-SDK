using UnityEngine;

namespace MMUCAVE
{
    /// <summary>
    /// All classes that inherit from InteractionObject respond to being tapped through implementation of OnTouch.
    /// <u>Requires a collider on the object</u>
    /// </summary>
    public abstract class InteractionObject : MonoBehaviour
    {
        /// <summary>
        /// All Interaction Objects must implement OnTouch as the reaction to any tap events.
        /// </summary>
        /// <example>
        /// <code>
        /// public override void OnTouch()
        /// {
        ///    CAVE.transform.position = transform.position;
        ///    CAVE.transform.rotation = Quaternion.Euler(rotationOffset);
        /// }
        /// </code>
        /// </example>
        public abstract void OnTouch();
    }
}
