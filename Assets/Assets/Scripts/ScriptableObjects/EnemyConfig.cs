using UnityEngine;

namespace TLS.SO
{
    [CreateAssetMenu(menuName = "TLS/Enemy Config", fileName = "EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public GameObject enemyPrefab;
        [Header("t=0 base values")]
        public float HP0 = 10f;
        public float Speed0 = 1f;
        public float DMG0 = 1f;
        public int CoinReward = 5;
    }
}

