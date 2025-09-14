using UnityEngine;

namespace TLS.Abilities
{
    // Absorbs incoming damage up to capacity and regenerates after delay.
    public class Shield : MonoBehaviour
    {
        public int capacity = 3;
        public float regenTime = 5f;
        public float startupDelay = 0f;

        private int _cur;
        private float _regenAt;

        private void Start()
        {
            TLS.Progression.UpgradeService.ApplyShield(this);
            _cur = capacity;
            _regenAt = Time.time + startupDelay;
        }

        private void Update()
        {
            if (_cur <= 0 && Time.time >= _regenAt)
            {
                _cur = capacity;
            }
        }

        // Returns leftover damage after absorption
        public int Absorb(int dmg)
        {
            // Gate by unlock state; if not unlocked, do nothing
            if (!TLS.Progression.UnlocksService.IsUnlocked(TLS.Progression.UnlocksService.Ability.Shield))
                return dmg;
            if (_cur <= 0) return dmg;
            int used = Mathf.Min(_cur, dmg);
            _cur -= used;
            int left = dmg - used;
            if (_cur <= 0)
            {
                _regenAt = Time.time + regenTime;
                TLS.Util.VFX.SpawnRing(transform.position, 1.2f, new Color(0.4f,0.8f,1f,0.6f), 0.35f);
            }
            return left;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, 1.2f);
        }
#endif
    }
}
