using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public GameObject enemyPrefab;
    public float HP0 = 10f;
    public float Speed0 = 1f;
    public float DMG0 = 1f;
    public int CoinReward = 5;
}

