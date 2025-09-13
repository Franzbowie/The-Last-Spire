using System.Collections;
using UnityEngine;
using TLS.Core;

namespace TLS.Abilities
{
    // Emits a line beam that slows enemies, no damage.
    public class BeamSlow : MonoBehaviour
    {
        public float interval = 4f;
        public float activeTime = 1.5f;
        public float length = 8f;
        public float width = 0.5f;
        public float slowFactor = 0.4f; // 60% slow

        private float _timer;
        private bool _wasRunning;
        private LineRenderer _lr;

        private void Awake()
        {
            _lr = gameObject.AddComponent<LineRenderer>();
            _lr.material = new Material(Shader.Find("Sprites/Default"));
            _lr.widthMultiplier = width;
            _lr.startColor = _lr.endColor = new Color(0.6f, 0.9f, 1f, 0.8f);
            _lr.enabled = false;
            _lr.positionCount = 2;
        }

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
                _timer = interval; // delay first beam when run starts
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = interval;
                StartCoroutine(Fire());
            }
        }

        private IEnumerator Fire()
        {
            float t = 0f;
            _lr.enabled = true;
            Transform target = FindNearestEnemy();
            Vector3 dir = target ? (target.position - transform.position).normalized : Vector3.right;
            while (t < activeTime)
            {
                t += Time.deltaTime;
                Vector3 a = transform.position;
                Vector3 b = a + dir * length;
                _lr.SetPosition(0, a);
                _lr.SetPosition(1, b);
                SlowAlongBeam(a, b);
                yield return null;
            }
            _lr.enabled = false;
        }

        private void SlowAlongBeam(Vector3 a, Vector3 b)
        {
            Vector2 ab = (b - a);
            float len2 = ab.sqrMagnitude; if (len2 < 0.001f) return;
            foreach (var e in GameObject.FindObjectsOfType<TLS.Combat.EnemyController>())
            {
                Vector2 ap = (Vector2)e.transform.position - (Vector2)a;
                float t = Mathf.Clamp01(Vector2.Dot(ap, ab) / len2);
                Vector2 proj = (Vector2)a + ab * t;
                float dist = Vector2.Distance((Vector2)e.transform.position, proj);
                if (dist <= width)
                {
                    e.ApplySlow(slowFactor, 0.2f);
                }
            }
        }

        private Transform FindNearestEnemy()
        {
            TLS.Combat.EnemyController best = null; float bestSqr = float.PositiveInfinity;
            foreach (var e in GameObject.FindObjectsOfType<TLS.Combat.EnemyController>())
            {
                float d = (e.transform.position - transform.position).sqrMagnitude;
                if (d < bestSqr) { bestSqr = d; best = e; }
            }
            return best ? best.transform : null;
        }
    }
}
