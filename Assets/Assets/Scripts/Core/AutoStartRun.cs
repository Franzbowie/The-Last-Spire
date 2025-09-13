using UnityEngine;

namespace TLS.Core
{
    // Helper to auto-start a run when scene loads (for quick prototyping)
    public class AutoStartRun : MonoBehaviour
    {
        private void Start()
        {
            if (GameManager.I != null)
            {
                GameManager.I.StartRun();
            }
        }
    }
}

