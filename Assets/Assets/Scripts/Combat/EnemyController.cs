using UnityEngine;
using TLS.Combat;

namespace TLS.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public float hp = 10f;
        public float speed = 1f;
        public float dmg = 1f;
        public int coinReward = 5;

        [Header("Telegraph")]
        public float warnRadius = 2.5f;
        public Color normalColor = Color.white;
        public Color warnColor = new Color(1f, 0.4f, 0.4f, 1f);

        private Transform tower;
        private SpriteRenderer sr;
        private float baseSpeed;
        private float slowUntil;
        private float slowFactor = 1f;

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            var th = FindObjectOfType<TowerHealth>();
            tower = th != null ? th.transform : null;
            baseSpeed = speed;
        }

        private void Update()
        {
            if (tower == null) return;

            float curSpeed = baseSpeed * ((Time.time < slowUntil) ? slowFactor : 1f);
            transform.position = Vector3.MoveTowards(transform.position, tower.position, curSpeed * Time.deltaTime);

            if (sr != null)
            {
                float dist = Vector3.Distance(transform.position, tower.position);
                sr.color = dist <= warnRadius ? warnColor : normalColor;
            }
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            if (c.TryGetComponent(out TowerHealth th))
            {
                th.TakeHit(Mathf.CeilToInt(dmg), this);
                Die(false);
            }
        }

        public void Die(bool killedByPlayer)
        {
            if (killedByPlayer && TLS.Progression.Coins.I != null)
            {
                int reward = coinReward;
                if (TLS.Progression.PassiveService.CoinsPlus) reward += 1;
                if (TLS.Progression.PassiveService.Extra && Random.value < 0.02f) reward += 50;
                TLS.Progression.Coins.I.Add(reward);
            }
            if (killedByPlayer)
            {
                TLS.Core.StatsService.IncKill();
            }
            Destroy(gameObject);
        }

        public void ApplySlow(float factor, float duration)
        {
            factor = Mathf.Clamp(factor, 0.05f, 1f);
            if (factor < slowFactor) slowFactor = factor; // take stronger slow
            if (Time.time + duration > slowUntil) slowUntil = Time.time + duration;
        }
    }
}
