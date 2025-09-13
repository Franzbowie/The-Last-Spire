using UnityEngine;

namespace TLS.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        public float speed = 10f;
        public float damage = 5f;
        public float life = 3f;
        public Vector2 dir = Vector2.right;
        public bool homing;
        public float homingTurnRate = 360f; // deg/sec
        public int pierce = 0;

        private Transform _target;

        private void Awake()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
            // simple trail
            var tr = gameObject.AddComponent<TrailRenderer>();
            tr.time = 0.25f;
            tr.startWidth = 0.08f;
            tr.endWidth = 0.01f;
            var shader = Shader.Find("Sprites/Default");
            tr.material = new Material(shader);
            tr.material.color = new Color(1f, 0.9f, 0.3f, 1f);
            var grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(new Color(1f,0.9f,0.3f,1f), 0f), new GradientColorKey(new Color(1f,0.6f,0.2f,1f), 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            );
            tr.colorGradient = grad;
        }

        public void SetTarget(Transform t)
        {
            _target = t;
            homing = t != null;
        }

        private void Update()
        {
            life -= Time.deltaTime;
            if (life <= 0f) { Destroy(gameObject); return; }

            if (homing && _target != null)
            {
                Vector2 to = ((Vector2)_target.position - (Vector2)transform.position).normalized;
                float maxStep = homingTurnRate * Mathf.Deg2Rad * Time.deltaTime;
                dir = Vector2.Lerp(dir, to, Mathf.Clamp01(maxStep));
            }

            transform.position += (Vector3)(dir.normalized * speed * Time.deltaTime);
            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, ang);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out TLS.Combat.EnemyController e))
            {
                float dmg = TLS.Util.Damage.WithCrit(damage);
                e.hp -= dmg;
                if (e.hp <= 0f) e.Die(true);
                if (TLS.Progression.PassiveService.Pierce)
                {
                    if (pierce <= 0) pierce = 1; // one extra by default
                }
                if (pierce > 0) { pierce--; }
                else { Destroy(gameObject); }
            }
        }
    }
}
