using UnityEngine;

namespace TLS.SO
{
    [CreateAssetMenu(menuName = "TLS/Difficulty Curve", fileName = "DifficultyCurve")]
    public class DifficultyCurve : ScriptableObject
    {
        [Header("Spawn")]
        public float SR0 = 0.5f; // base spawns per second
        public float c = 0.02f;   // spawn rate growth per second

        [Header("Enemy scaling")] 
        public float a = 0.01f;   // HP growth factor
        public float b = 1.1f;    // HP exponent
        public float d = 0.0f;    // speed growth
        public float e = 0.01f;   // damage growth
    }
}

