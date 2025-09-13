using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TLS.Inputs
{
    public class AimProvider : MonoBehaviour
    {
        private Vector3 lastAim;

        public Vector3 AimPoint
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                // Pointer via new Input System
                if (Camera.main != null)
                {
                    if (Mouse.current != null)
                    {
                        Vector2 mp = Mouse.current.position.ReadValue();
                        lastAim = Camera.main.ScreenToWorldPoint(new Vector3(mp.x, mp.y, 0f));
                    }
                    if (Touchscreen.current != null)
                    {
                        var touch = Touchscreen.current.primaryTouch;
                        if (touch != null && touch.press.isPressed)
                        {
                            Vector2 tp = touch.position.ReadValue();
                            lastAim = Camera.main.ScreenToWorldPoint(new Vector3(tp.x, tp.y, 0f));
                        }
                    }
                }
#else
                // Legacy Input fallback
                if (Camera.main != null)
                {
                    lastAim = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                    if (UnityEngine.Input.touchCount > 0)
                        lastAim = Camera.main.ScreenToWorldPoint(UnityEngine.Input.GetTouch(0).position);
                }
#endif
                lastAim.z = 0f;
                return lastAim;
            }
        }

        public bool IsAimEngaged
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return Touchscreen.current != null && Touchscreen.current.primaryTouch != null && Touchscreen.current.primaryTouch.press.isPressed;
#else
                return UnityEngine.Input.touchCount > 0;
#endif
            }
        }
    }
}
