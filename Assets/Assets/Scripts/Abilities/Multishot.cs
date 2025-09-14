using UnityEngine;
using TLS.Core;
using TLS.Progression;

namespace TLS.Abilities
{
    // Periodically fires a fan of straight projectiles towards nearest enemy direction.
    public class Multishot : MonoBehaviour
    {
        public float interval = 3f;
        public int count = 5;
        public float spreadDeg = 40f;
        public float damage = 3f;
        public float speed = 14f;
        public float life = 2.5f;

        private float _timer;
        private bool _wasRunning;

        private void Start()
        {
            TLS.Progression.UpgradeService.ApplyMultishot(this);
        }

        private void Update()
        {
            bool running = GameManager.I != null && GameManager.I.Current == GameManager.State.Run;
            bool unlocked = UnlocksService.IsUnlocked(UnlocksService.Ability.Multishot);
            if (!running)
            {
                _wasRunning = false;
                return;
            }
            if (!unlocked)
            {
                // Ensure it doesn't act if not unlocked, even if the component is present
                _wasRunning = false;
                return;
            }
            if (!_wasRunning)
            {
                _wasRunning = true;
                _timer = interval; // start with full interval when run begins
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = interval;
                FireFan();
            }
        }

        private void FireFan()
        {
            Vector2 dir = FindDirection();
            if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
            int n = Mathf.Max(1, count);
            float totalSpread = spreadDeg * Mathf.Deg2Rad;
            for (int i = 0; i < n; i++)
            {
                float t = n == 1 ? 0f : (i / (float)(n - 1)) - 0.5f;
                float ang = t * totalSpread;
                Vector2 d = new Vector2(
                    dir.x * Mathf.Cos(ang) - dir.y * Mathf.Sin(ang),
                    dir.x * Mathf.Sin(ang) + dir.y * Mathf.Cos(ang)
                ).normalized;
                SpawnBullet(d);
            }
        }

        private void SpawnBullet(Vector2 d)
        {
            var go = new GameObject("MS_Bullet");
            go.transform.position = transform.position;
            var col = go.AddComponent<CircleCollider2D>(); col.isTrigger = true; col.radius = 0.1f;
            var p = go.AddComponent<TLS.Projectiles.Projectile>();
            p.damage = damage;
            p.speed = speed;
            p.life = life;
            p.dir = d;
            p.homing = false;
        }

        private Vector2 FindDirection()
        {
            TLS.Combat.EnemyController best = null;
            float bestSqr = float.PositiveInfinity;
            foreach (var e in GameObject.FindObjectsOfType<TLS.Combat.EnemyController>())
            {
                float d = (e.transform.position - transform.position).sqrMagnitude;
                if (d < bestSqr) { bestSqr = d; best = e; }
            }
            return best ? (best.transform.position - transform.position).normalized : Vector2.right;
        }
    }
}
