using UnityEngine;
using TLS.Core;
using TLS.SO;

namespace TLS.Combat
{
    public class EnemySpawner : MonoBehaviour
    {
        public EnemyConfig cfg;
        public DifficultyCurve curve;
        public float spawnRadius = 10f;
        [Tooltip("If no EnemyConfig/prefab is assigned, create a simple runtime enemy so spawning still works.")]
        public bool autoCreateFallbackEnemy = true;

        private float t;
        private float spawnTimer;
        private static Sprite _fallbackSprite;
        private bool _runInit;

        private void Update()
        {
            if (GameManager.I == null || GameManager.I.Current != GameManager.State.Run)
                return;

            float dt = Time.deltaTime;

            // Apply start-minute offset once at run start
            if (!_runInit)
            {
                _runInit = true;
                int startMin = TLS.Progression.PassiveService.StartMinute;
                if (startMin > 0)
                {
                    float offset = startMin * 60f;
                    t += offset;
                    // ensure first spawn happens promptly after run begins
                    spawnTimer = 0f;
                }
            }

            t += dt;

            float spawnRate = curve != null ? curve.SR0 * (1f + curve.c * t) : 0.5f;
            spawnRate = Mathf.Max(0.01f, spawnRate);

            spawnTimer -= dt;
            if (spawnTimer <= 0f)
            {
                SpawnEnemy(t);
                spawnTimer = 1f / spawnRate;
            }
        }

        private void SpawnEnemy(float time)
        {
            GameObject go = null;
            if (cfg != null && cfg.enemyPrefab != null)
            {
                go = Instantiate(cfg.enemyPrefab, RandomEdgePos(), Quaternion.identity);
            }
            else if (autoCreateFallbackEnemy)
            {
                // Silent fallback - no warning needed since this is expected behavior
                go = CreateRuntimeEnemy();
                go.transform.position = RandomEdgePos();
            }
            else
            {
                Debug.LogWarning("EnemySpawner: No EnemyConfig/prefab; set cfg and enemyPrefab to enable spawning.");
                return;
            }

            // Additional safety check
            if (go == null)
            {
                Debug.LogError("EnemySpawner: Failed to create enemy GameObject. Skipping spawn.");
                return;
            }

            var e = go.GetComponent<EnemyController>();
            if (e == null) e = go.AddComponent<EnemyController>();

            // Use config values if available, otherwise use defaults
            float hp0 = cfg != null ? cfg.HP0 : 10f;
            float s0 = cfg != null ? cfg.Speed0 : 1f;
            float dmg0 = cfg != null ? cfg.DMG0 : 1f;
            int coinReward = cfg != null ? cfg.CoinReward : 5;

            if (curve != null)
            {
                e.hp = hp0 * Mathf.Pow(1f + curve.a * time, curve.b);
                e.dmg = dmg0 * (1f + curve.e * time);
                e.speed = s0 * (1f + curve.d * time);
            }
            else
            {
                e.hp = hp0;
                e.dmg = dmg0;
                e.speed = s0;
            }

            e.coinReward = coinReward;
        }

        private Vector3 RandomEdgePos()
        {
            float ang = Random.Range(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f) * spawnRadius;
        }

        private GameObject CreateRuntimeEnemy()
        {
            var go = new GameObject("Enemy_Runtime");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = GetFallbackSprite();
            sr.color = new Color(0.9f, 0.2f, 0.2f, 1f);
            var col = go.AddComponent<CircleCollider2D>(); col.isTrigger = true; col.radius = 0.35f;
            var rb = go.AddComponent<Rigidbody2D>(); rb.bodyType = RigidbodyType2D.Kinematic; rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            go.AddComponent<EnemyController>();
            return go;
        }

        private static Sprite GetFallbackSprite()
        {
            if (_fallbackSprite != null) return _fallbackSprite;
            var tex = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            var colors = new Color32[8 * 8];
            for (int i = 0; i < colors.Length; i++) colors[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(colors); tex.Apply();
            _fallbackSprite = Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 16f);
            _fallbackSprite.name = "EnemyFallbackSprite";
            return _fallbackSprite;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(1f, 0.5f, 0.2f, 0.5f);
            UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.forward, spawnRadius);
        }
#endif
    }
}
