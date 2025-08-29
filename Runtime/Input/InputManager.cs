using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

namespace MMUCAVE
{
    public class InputManager : MonoBehaviour //Could make this a singleton to get a reference to anywhere?
    {
        #region Events

        public delegate void StartTouch(Vector2 touchPosition, float time);

        public event StartTouch OnStartTouch;

        public delegate void EndTouch(Vector2 touchPosition, float time);

        public event EndTouch OnEndTouch;

        #endregion

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
            playerControls.Touch.PrimaryContact.started +=
                ctx => StartTouchPrimary(
                    ctx); //subscribes start touch function to touch started and passes in callback context
            playerControls.Touch.PrimaryContact.canceled +=
                ctx => EndTouchPrimary(
                    ctx); //subscribes end touch function to touch cancelled and passes in callback context
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            //if something is subscribed to this event, call the started function.
            if (OnStartTouch != null)
                OnStartTouch(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.startTime);
            //passes in the position screen was touched at and the time the input was started
        }

        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            //if something is subscribed to this event, call the ended function.
            if (OnEndTouch != null)
                OnEndTouch(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.time);
            //passes in the position screen was last at and the time the input was ended
        }

        public Vector2 PrimaryPosition()
        {
            return playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        }
        
    }
}
