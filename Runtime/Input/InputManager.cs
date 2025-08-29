using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

namespace MMUCAVE
{
    public class InputManager : MonoBehaviour //Could make this a singleton to get a reference to anywhere?
    {
        [SerializeField] private float minimumDistance = 100f;
        [SerializeField] private float inputUpdateWait = 0.1f;
        [SerializeField] private float directionThreshold = .9f;
        [SerializeField] private float tapTimeThreshold = 0.01f;
        [SerializeField] CAVEInputManager caveInputManager;
        private Vector2 _lastPosition, _direction;
        private float _startTime;
        private Coroutine _coroutine;

        private GestureControls playerControls;

        // Start is called before the first frame update
        void Awake()
        {
            playerControls = new GestureControls();
        }

        // Update is called once per frame
        void OnEnable()
        {
            playerControls.Enable();
        }

        void OnDisable()
        {
            playerControls.Disable();
        }

        void Start()
        {
            playerControls.Touch.PrimaryContact.started += ctx => StartTouch(); //subscribes start touch function to touch started
            playerControls.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx); //subscribes end touch function to touch cancelled
        }
        

        public Vector2 PrimaryPosition()
        {
            return playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        }

        #region Swipe Detection

        void StartTouch()
        {   //reset all values
            _lastPosition = PrimaryPosition();
            _startTime = Time.time;
            _direction = Vector2.zero;
            _coroutine = StartCoroutine(TouchUpdate());
        }

        private IEnumerator TouchUpdate()
        {
            while (true)
            {   //update values
                _direction = Vector2.zero;
                DetectSwipe();
                _lastPosition = PrimaryPosition();
                yield return new WaitForSeconds(inputUpdateWait);
            }
        }

        void EndTouch(InputAction.CallbackContext context)
        {   
            Debug.Log(Mathf.Abs(_startTime - Time.time));
            StopCoroutine(_coroutine);
            if (Mathf.Abs(_startTime - Time.time) < tapTimeThreshold)//compare start and current time
            {
                Debug.Log("Tap");
                caveInputManager.HandleTouchActions(PrimaryPosition(), InputSwitchUtility.TouchTypes.Touchables);
            }
            else
            {
                Debug.Log("Hold");
            }
        }

        void DetectSwipe()
        {
            //touch is a swipe if it has moved so far since the last check.
            if (Vector2.Distance(_lastPosition, PrimaryPosition()) >= minimumDistance)
            {
                Debug.DrawLine(_lastPosition, PrimaryPosition(), Color.red, 5f);
                _direction = (PrimaryPosition() - _lastPosition).normalized;
                SwipeDirection(_direction);
            }
        }

        void SwipeDirection(Vector2 direction)
        {
            //uses the dot product to determine how similar the touch direction is to each cardinal direction.
            if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
            {
                caveInputManager.RotateCAVERight();
            }

            if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
            {
                caveInputManager.RotateCAVELeft();
            }
        }


        #endregion
    }
}
