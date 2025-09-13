using UnityEngine;
using TLS.Core;

namespace TLS.Combat
{
    public class TowerHealth : MonoBehaviour
    {
        [Min(1)] public int segments = 1;

        private void Start()
        {
            segments += TLS.Progression.PassiveService.TowerHPBonus;
        }

        public void TakeHit(int dmg = 1, EnemyController src = null)
        {
            // Shield absorbs first
            var sh = GetComponent<TLS.Abilities.Shield>();
            if (sh != null)
            {
                dmg = sh.Absorb(dmg);
            }
            if (dmg <= 0) return;
            segments -= Mathf.Max(1, dmg);
            if (src != null && TLS.Progression.PassiveService.Spikes)
            {
                src.hp -= 1f; if (src.hp <= 0f) src.Die(true);
            }
            if (segments <= 0)
            {
                segments = 0;
                if (GameManager.I != null)
                    GameManager.I.OnTowerDead();
            }
        }
    }
}
