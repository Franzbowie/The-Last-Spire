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

        [Header("Attack")]
        public float attackDelay = 1f; // Delay before attacking when in range
        public float attackRadius = 0.5f; // Distance from tower to start attack sequence

        private Transform tower;
        private SpriteRenderer sr;
        private float baseSpeed;
        private float slowUntil;
        private float slowFactor = 1f;
        private bool isInAttackRange = false;
        private float attackTimer = 0f;
        private bool hasAttacked = false;

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

            float dist = Vector3.Distance(transform.position, tower.position);
            bool wasInAttackRange = isInAttackRange;
            isInAttackRange = dist <= attackRadius;

            // Only move if not in attack range or not yet attacked
            if (!isInAttackRange || !hasAttacked)
            {
                float curSpeed = baseSpeed * ((Time.time < slowUntil) ? slowFactor : 1f);
                transform.position = Vector3.MoveTowards(transform.position, tower.position, curSpeed * Time.deltaTime);
            }

            // Update visual state
            if (sr != null)
            {
                if (isInAttackRange && !hasAttacked)
                {
                    // Pulsing red effect during attack countdown
                    float pulseIntensity = 0.5f + 0.5f * Mathf.Sin(Time.time * 10f);
                    float timeLeft = attackTimer / attackDelay;
                    Color currentWarnColor = Color.Lerp(warnColor, Color.red, timeLeft);
                    currentWarnColor.a = pulseIntensity;
                    sr.color = currentWarnColor;
                }
                else
                {
                    sr.color = normalColor;
                }
            }

            // Handle attack logic
            if (isInAttackRange && !hasAttacked)
            {
                if (!wasInAttackRange)
                {
                    // Just entered attack range, start timer
                    attackTimer = attackDelay;
                }
                else
                {
                    // Count down attack timer
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        // Time to attack!
                        AttackTower();
                    }
                }
            }
        }

        private void AttackTower()
        {
            if (hasAttacked) return;
            
            hasAttacked = true;
            var th = FindObjectOfType<TowerHealth>();
            if (th != null)
            {
                th.TakeHit(Mathf.CeilToInt(dmg), this);
            }
            Die(false);
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            // This method is now only used for other triggers, not tower attacks
            // Tower attacks are handled by the attack timer system
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
