using System.Collections;
using UnityEngine;
using TLS.Inputs;
using TLS.Core;

namespace TLS.Abilities
{
    public class SkyStrike : MonoBehaviour
    {
        public float interval = 0.5f;
        public float radius = 1f;
        public float damage = 10f;
        public int count = 1;
        [Header("Projectile")]
        public GameObject projectilePrefab; // optional; if null, runtime fallback used
        public float spawnHeight = 10f;
        public float fallSpeed = 20f;

        private float timer;
        private AimProvider aim;
        private LineRenderer indicator;
        private const int IndicatorSteps = 48;

        private void Start()
        {
            aim = FindObjectOfType<AimProvider>();
            TLS.Progression.UpgradeService.ApplySkyStrike(this);
            SetupIndicator();
        }

        private void Update()
        {
            // Hide indicator and do nothing if not in Run state
            if (GameManager.I == null || GameManager.I.Current != GameManager.State.Run)
            {
                if (indicator != null) indicator.enabled = false;
                return;
            }

            timer -= Time.deltaTime;
            bool shouldFire;
#if UNITY_ANDROID || UNITY_IOS
            shouldFire = (aim != null && aim.IsAimEngaged);
#else
            shouldFire = true; // PC/editor auto-fire
#endif
            if (shouldFire && timer <= 0f)
            {
                timer = interval;
                var p = aim != null ? aim.AimPoint : transform.position;
                StartCoroutine(SpawnBurst(p));
            }

            if (indicator != null) indicator.enabled = true;
            UpdateIndicator(aim != null ? aim.AimPoint : transform.position);
        }

        private IEnumerator SpawnBurst(Vector3 p)
        {
            int n = Mathf.Max(1, count);
            for (int i = 0; i < n; i++)
            {
                SpawnProjectile(p);
            }
            yield return null;
        }

        private void SpawnProjectile(Vector3 p)
        {
            Vector3 spawnPos = p + Vector3.up * spawnHeight;
            GameObject go = null;
            if (projectilePrefab != null)
            {
                go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                go = new GameObject("SkyStrikeProjectile");
                go.transform.position = spawnPos;
            }

            var proj = go.GetComponent<TLS.Projectiles.SkyStrikeProjectile>();
            if (proj == null) proj = go.AddComponent<TLS.Projectiles.SkyStrikeProjectile>();
            proj.target = p;
            proj.radius = radius;
            proj.damage = damage;
            proj.fallSpeed = fallSpeed;
            proj.spawnHeight = spawnHeight;
            // use anchor mode: fall straight down relative to cursor and stay attached to it
            proj.useAnchor = true;
            proj.aim = aim;
        }

        private void SetupIndicator()
        {
            var go = new GameObject("SkyStrikeIndicator");
            go.transform.SetParent(null); // world space
            indicator = go.AddComponent<LineRenderer>();
            indicator.loop = true;
            indicator.positionCount = IndicatorSteps;
            indicator.widthMultiplier = 0.05f;
            indicator.material = new Material(Shader.Find("Sprites/Default"));
            indicator.startColor = indicator.endColor = new Color(1f, 0.9f, 0.3f, 0.8f);
        }

        private void UpdateIndicator(Vector3 center)
        {
            if (indicator == null) return;
            for (int i = 0; i < IndicatorSteps; i++)
            {
                float t = (i / (float)IndicatorSteps) * Mathf.PI * 2f;
                indicator.SetPosition(i, center + new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f));
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.9f, 0.3f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
