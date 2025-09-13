using UnityEngine;
using UnityEngine.SceneManagement;

namespace TLS.Core
{
    public static class RunStarter
    {
        private static bool _pendingAutoStart;

        public static void SetAutoStartNextScene()
        {
            _pendingAutoStart = true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            var go = new GameObject("RunStarter");
            Object.DontDestroyOnLoad(go);
            go.AddComponent<RunStarterBehaviour>();
        }

        private class RunStarterBehaviour : MonoBehaviour
        {
            private void Start()
            {
                if (_pendingAutoStart && GameManager.I != null)
                {
                    _pendingAutoStart = false;
                    GameManager.I.StartRun();
                }
            }
        }
    }
}

