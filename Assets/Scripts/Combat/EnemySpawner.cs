using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyConfig cfg;
    public DifficultyCurve curve;

    float t;
    float spawnTimer;

    void Update()
    {
        if (GameManager.I == null || GameManager.I.Current != GameManager.State.Run)
            return;

        t += Time.deltaTime;
        float spawnRate = curve.SR0 * (1f + curve.c * t);
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy(t);
            spawnTimer = 1f / spawnRate;
        }
    }

    void SpawnEnemy(float time)
    {
        var prefab = cfg.enemyPrefab;
        var pos = RandomEdgePos();
        var e = Instantiate(prefab, pos, Quaternion.identity).GetComponent<EnemyController>();
        e.hp = cfg.HP0 * Mathf.Pow(1f + curve.a * time, curve.b);
        e.dmg = cfg.DMG0 * (1f + curve.e * time);
        e.speed = cfg.Speed0 * (1f + curve.d * time);
    }

    Vector3 RandomEdgePos()
    {
        var cam = Camera.main;
        if (cam == null) return Vector3.zero;
        var view = new Vector2(Random.value, Random.value);
        view = view * 2f - Vector2.one;
        Vector3 pos = cam.ViewportToWorldPoint(new Vector3(view.x, view.y, cam.nearClipPlane));
        pos.z = 0f;
        return pos.normalized * 10f; // rough circle
    }
}

