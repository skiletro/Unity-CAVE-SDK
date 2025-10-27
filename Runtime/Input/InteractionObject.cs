using UnityEngine;

namespace MMUCAVE
{
    /// <summary>
    ///     All classes that inherit from InteractionObject respond to being tapped through implementation of OnTouch.
    ///     (Requires a collider on the object)
    /// </summary>
    public abstract class InteractionObject : MonoBehaviour
    {

        [Tooltip("The percentage of similarity a swipe must have to register a cardinal direction")]
        [SerializeField]
        private float directionThreshold = .9f;

        /// <summary>
        ///     Override this to inform what happens when this object is tapped.
        /// </summary>
        /// <example>
        ///     <code>
        /// public override void OnTouch()
        /// {
        ///    CAVE.transform.position = transform.position;
        ///    CAVE.transform.rotation = Quaternion.Euler(rotationOffset);
        /// }
        /// </code>
        /// </example>
        public abstract void OnTouch();

        public abstract void OnSwipe(Vector2 direction);
        public abstract void OnRawSwipe(Vector2 direction);
        public abstract void OnHold();


        public void SwipeDirection(Vector2 direction)
        {
            OnRawSwipe(direction);
            // Uses the dot product to determine how similar the touch direction is to each cardinal direction.
            if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
            {
                direction = Vector2.down; // If swiped right, rotate right
            }

            if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
            {
                direction = Vector2.up; // If swiped left, rotate left
            }
            OnSwipe(direction);
        }
    }
