using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections;
using System.Collections.Generic;

namespace MMUCAVE
{
    public class EnhancedInputBehaviour : MonoBehaviour
    {
        private static int maxInputCount = 10;
        [SerializeField] private float minimumDistance = 100f;
        [SerializeField] private float inputUpdateWait = 0.1f;
        [SerializeField] private float directionThreshold = .9f;
        [SerializeField] private float tapTimeThreshold = 0.01f;
        [SerializeField] CAVEInputManager caveInputManager;
        private Vector2[] _lastPositions = new Vector2[maxInputCount], _directions = new Vector2[maxInputCount];
        private float[] _startTimes = new float[maxInputCount];
        [SerializeField]private Coroutine[] _coroutines = new Coroutine[maxInputCount];

        protected void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += FingerDown;
            Touch.onFingerUp += FingerUp;
        }

        protected void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            Touch.onFingerDown -= FingerDown;
            Touch.onFingerUp -= FingerUp;
        }


        private void FingerDown(Finger finger){
            _lastPositions[finger.index] = finger.currentTouch.screenPosition;
            _startTimes[finger.index] = Time.time;
            _directions[finger.index] = Vector2.zero;
            _coroutines[finger.index] = StartCoroutine(TouchUpdate(finger));
        }

        private void FingerUp(Finger finger){
            Debug.Log(Mathf.Abs(_startTimes[finger.index] - Time.time));
            StopCoroutine(_coroutines[finger.index]);
            if (Mathf.Abs(_startTimes[finger.index] - Time.time) < tapTimeThreshold)//compare start and current time
            {
                Debug.Log("Tap");
                caveInputManager.HandleTouchActions(finger.currentTouch.screenPosition, InputSwitchUtility.TouchTypes.Touchables);
            }
            else
            {
                Debug.Log("Hold");
            }
        }
         #region Swipe Detection

        private IEnumerator TouchUpdate(Finger finger)
        {
            while (true)
            {   //update values
                _directions[finger.index] = Vector2.zero;
                DetectSwipe(finger);
                _lastPositions[finger.index] = finger.currentTouch.screenPosition;
                yield return new WaitForSeconds(inputUpdateWait);
            }
        }

        void DetectSwipe(Finger finger)
        {
            //touch is a swipe if it has moved so far since the last check.
            if (Vector2.Distance(_lastPositions[finger.index], finger.currentTouch.screenPosition) >= minimumDistance)
            {
                _directions[finger.index] = (finger.currentTouch.screenPosition - _lastPositions[finger.index]).normalized;
                SwipeDirection(_directions[finger.index]);
            }
            else{
                caveInputManager.HandleTouchActions(finger.currentTouch.screenPosition, InputSwitchUtility.TouchTypes.Touchables);
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
