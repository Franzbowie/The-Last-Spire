using System.Collections;
using UnityEngine;
using TLS.Core;

namespace TLS.Abilities
{
    // Simple periodic global AoE tick. Attach to Tower.
    public class AoEEMI : MonoBehaviour
    {
        public float interval = 3f;
        public float radius = 8f; // screen-ish
        public float damage = 5f;
        public float startupDelay = 1f;

        private float _timer;
        private bool _wasRunning;

        private void Start()
        {
            _timer = startupDelay;
            TLS.Progression.UpgradeService.ApplyAoEEMI(this);
        }

        private void Update()
        {
            bool running = GameManager.I != null && GameManager.I.Current == GameManager.State.Run;
            bool unlocked = TLS.Progression.UnlocksService.IsUnlocked(TLS.Progression.UnlocksService.Ability.AoEEMI);
            if (!running)
            {
                _wasRunning = false;
                return;
            }
            if (!unlocked)
            {
                _wasRunning = false;
                return;
            }
            if (!_wasRunning)
            {
                _wasRunning = true;
                // keep whatever startupDelay set in Start via _timer
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = interval;
                StartCoroutine(Tick());
            }
        }

        private IEnumerator Tick()
        {
            // Optional tiny delay for telegraphing could be added here
            yield return null;
            var p = transform.position;
            TLS.Util.VFX.Explosion(p, radius, 0.15f);
            var hits = Physics2D.OverlapCircleAll(p, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].TryGetComponent(out TLS.Combat.EnemyController e))
                {
                    float dmg = TLS.Util.Damage.WithCrit(damage);
                    e.hp -= dmg;
                    if (e.hp <= 0f) e.Die(true);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.2f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
