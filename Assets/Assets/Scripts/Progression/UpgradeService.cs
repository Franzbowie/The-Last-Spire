using UnityEngine;

namespace TLS.Progression
{
    // Minimal upgrades for SkyStrike: damage, radius, interval, count
    public static class UpgradeService
    {
        private const string SkyStrikePrefix = "upg.sky";

        public enum SkyField { Damage, Radius, Interval, Count }

        private static string Key(SkyField f) => $"{SkyStrikePrefix}.{f.ToString().ToLower()}";

        public static int GetLevel(SkyField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(SkyField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }

        // Price curve: base * (level+1)
        public static int GetPrice(SkyField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                SkyField.Damage => 50,
                SkyField.Radius => 60,
                SkyField.Interval => 80,
                SkyField.Count => 120,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static bool Buy(SkyField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        // Apply levels to a specific SkyStrike instance
        public static void ApplySkyStrike(TLS.Abilities.SkyStrike sky)
        {
            if (sky == null) return;
            int dmgL = GetLevel(SkyField.Damage);
            int radL = GetLevel(SkyField.Radius);
            int intL = GetLevel(SkyField.Interval);
            int cntL = GetLevel(SkyField.Count);

            sky.damage = 10f + 5f * dmgL;            // +5 dmg per level
            sky.radius = 1f + 0.25f * radL;         // +0.25 radius per level
            sky.interval = 0.5f * Mathf.Pow(0.9f, intL); // 10% faster per level
            sky.count = 1 + cntL;                   // +1 projectile per level
        }
    }
}

