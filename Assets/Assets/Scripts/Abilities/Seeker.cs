using UnityEngine;
using TLS.Core;

namespace TLS.Abilities
{
    // Fires a homing projectile at nearest enemy every interval.
    public class Seeker : MonoBehaviour
    {
        public float interval = 0.5f;
        public float damage = 5f;
        public float speed = 12f;
        public float life = 3f;

        private float _timer;
        private bool _wasRunning;

        private void Update()
        {
            bool running = GameManager.I != null && GameManager.I.Current == GameManager.State.Run;
            if (!running)
            {
                _wasRunning = false;
                return;
            }
            if (!_wasRunning)
            {
                _wasRunning = true;
                _timer = interval; // delay first shot when run starts
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = interval;
                Fire();
            }
        }

        private void Fire()
        {
            var target = FindNearestEnemy();
            if (target == null) return;

            var go = new GameObject("SeekerProjectile");
            go.transform.position = transform.position;
            var col = go.AddComponent<CircleCollider2D>(); col.isTrigger = true; col.radius = 0.1f;
            var p = go.AddComponent<TLS.Projectiles.Projectile>();
            p.damage = damage;
            p.speed = speed;
            p.life = life;
            p.dir = (target.position - transform.position).normalized;
            p.homing = true;
            p.SetTarget(target);
        }

        private Transform FindNearestEnemy()
        {
            TLS.Combat.EnemyController best = null;
            float bestSqr = float.PositiveInfinity;
            foreach (var e in GameObject.FindObjectsOfType<TLS.Combat.EnemyController>())
            {
                float d = (e.transform.position - transform.position).sqrMagnitude;
                if (d < bestSqr) { bestSqr = d; best = e; }
            }
            return best ? best.transform : null;
        }
    }
}
