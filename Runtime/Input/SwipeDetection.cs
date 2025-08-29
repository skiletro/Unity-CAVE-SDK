using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMUCAVE
{
    public class SwipeDetection : MonoBehaviour
    {
        [SerializeField]
        private float minimumDistance = .2f;
        /*[SerializeField] 
        private GameObject trail;*/
        [SerializeField] 
        private float maximumTime = 1f;
        [SerializeField] 
        private float directionThreshold = .9f;
        
        [SerializeField] private InputManager inputManager;
        private Vector2 _startPosition,  _endPosition;
        private float _startTime, _endTime;
        //private Coroutine _coroutine;

        void OnEnable()
        {
            inputManager.OnStartTouch += SwipeStart;//Subscribes to events called in InputManager
            inputManager.OnEndTouch += SwipeEnd;
        }

        void OnDisable()
        {
            inputManager.OnStartTouch -= SwipeStart;//Unsubscribes from events in InputManager
            inputManager.OnEndTouch -= SwipeEnd;
        }

        void SwipeStart(Vector2 touchPosition, float time)
        {
            _startPosition =  touchPosition;
            _startTime = time;
            /*trail.SetActive(true);
            trail.transform.position = _startPosition;
            _coroutine = StartCoroutine(TrailUpdate());*/
        }

        /*private IEnumerator TrailUpdate()
        {
            while (true)
            {
                trail.transform.position = inputManager.PrimaryPosition();
                yield return null;//null so that it is executed every frame while running
            }
        }*/
        
        void SwipeEnd(Vector2 touchPosition, float time)
        {
            _endPosition =  touchPosition;
            _endTime = time;
            DetectSwipe();
            //StopCoroutine(_coroutine);
            //trail.SetActive(false);
        }

        void DetectSwipe()
        { //if touch action falls within our bounds of required speed then act as a swipe 
            if (Vector2.Distance(_startPosition, _endPosition) >= minimumDistance &&
                _endTime - _startTime <= maximumTime)
            {
                Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
                Vector2 direction = (_endPosition - _startPosition).normalized;
                SwipeDirection(direction);
            }
        }

        void SwipeDirection(Vector2 direction)
        {
            //uses the dot product to determine how similar the touch direction is to each cardinal direction.
            if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
            {
                Debug.Log("Swipe Right");
            }
            if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
            {
                Debug.Log("Swipe Left");
            }
        }
    }
}
