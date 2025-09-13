using UnityEngine;

namespace TLS.Util
{
    public static class Damage
    {
        public static float WithCrit(float baseDamage)
        {
            if (!TLS.Progression.PassiveService.Crit) return baseDamage;
            return (Random.value < 0.05f) ? baseDamage * 2f : baseDamage;
        }
    }
}

