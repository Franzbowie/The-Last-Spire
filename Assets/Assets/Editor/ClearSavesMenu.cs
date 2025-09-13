using UnityEditor;
using UnityEngine;

namespace TLS.EditorTools
{
    public static class ClearSavesMenu
    {
        [MenuItem("Tools/Clear Saves (PlayerPrefs)")]
        public static void ClearSaves()
        {
            TLS.Core.GameManager.ClearAllSaves();
            Debug.Log("All saves cleared (PlayerPrefs.DeleteAll)");
        }

        [MenuItem("Tools/Clear Saves (PlayerPrefs)", true)]
        public static bool ValidateClearSaves()
        {
            return true; // always enabled
        }
    }
}

