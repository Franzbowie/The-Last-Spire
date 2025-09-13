using UnityEngine;

namespace TLS.UI
{
    // Hook Start button to StartRun; optionally hide menu panel
    public class MenuController : MonoBehaviour
    {
        public GameObject menuRoot;

        public void OnClickStart()
        {
            TLS.Core.GameManager.I.StartRun();
            if (menuRoot != null) menuRoot.SetActive(false);
        }
    }
}

