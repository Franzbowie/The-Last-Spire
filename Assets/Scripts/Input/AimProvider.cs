using UnityEngine;

public class AimProvider : MonoBehaviour
{
    Vector3 lastAim;

    public Vector3 AimPoint
    {
        get
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            lastAim = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else
            if (Input.touchCount > 0)
                lastAim = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif
            lastAim.z = 0f;
            return lastAim;
        }
    }
}

