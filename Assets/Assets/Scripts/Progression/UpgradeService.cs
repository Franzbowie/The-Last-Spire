using UnityEngine;

namespace TLS.Progression
{
    // Upgrades for weapons: SkyStrike, Seeker, Multishot, AoEEMI, Beam, Shield
    public static class UpgradeService
    {
        private const string SkyStrikePrefix = "upg.sky";
        private const string SeekerPrefix = "upg.seeker";
        private const string MultiPrefix = "upg.multi";
        private const string EmiPrefix = "upg.emi";
        private const string BeamPrefix = "upg.beam";
        private const string ShieldPrefix = "upg.shield";
        // Future placeholders (not implemented abilities yet): swords, runes, bouncer, mortar
        private const string SwordsPrefix = "upg.swords";
        private const string RunesPrefix = "upg.runes";
        private const string BouncerPrefix = "upg.bouncer";
        private const string MortarPrefix = "upg.mortar";

        public enum SkyField { Damage, Radius, Interval, Count }
        public enum SeekerField { Damage, Interval }
        public enum MultiField { Damage, Interval, Count }
        public enum EmiField { Damage, Interval }
        public enum BeamField { Slow, Duration, Interval }
        public enum ShieldField { Capacity, Regen, Startup }
        public enum SwordsField { Damage, Rate }
        public enum RunesField { Damage, Count, Speed, Interval }
        public enum BouncerField { Damage, Duration, Interval, Speed }
        public enum MortarField { Damage, Interval }

        private static string Key(SkyField f) => $"{SkyStrikePrefix}.{f.ToString().ToLower()}";
        private static string Key(SeekerField f) => $"{SeekerPrefix}.{f.ToString().ToLower()}";
        private static string Key(MultiField f) => $"{MultiPrefix}.{f.ToString().ToLower()}";
        private static string Key(EmiField f) => $"{EmiPrefix}.{f.ToString().ToLower()}";
        private static string Key(BeamField f) => $"{BeamPrefix}.{f.ToString().ToLower()}";
        private static string Key(ShieldField f) => $"{ShieldPrefix}.{f.ToString().ToLower()}";
        private static string Key(SwordsField f) => $"{SwordsPrefix}.{f.ToString().ToLower()}";
        private static string Key(RunesField f) => $"{RunesPrefix}.{f.ToString().ToLower()}";
        private static string Key(BouncerField f) => $"{BouncerPrefix}.{f.ToString().ToLower()}";
        private static string Key(MortarField f) => $"{MortarPrefix}.{f.ToString().ToLower()}";

        public static int GetLevel(SkyField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(SkyField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(SeekerField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(SeekerField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(MultiField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(MultiField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(EmiField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(EmiField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(BeamField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(BeamField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(ShieldField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(ShieldField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(SwordsField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(SwordsField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(RunesField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(RunesField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(BouncerField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(BouncerField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }
        public static int GetLevel(MortarField f) => PlayerPrefs.GetInt(Key(f), 0);
        private static void SetLevel(MortarField f, int lvl) { PlayerPrefs.SetInt(Key(f), Mathf.Max(0, lvl)); PlayerPrefs.Save(); }

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

        public static int GetPrice(SeekerField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                SeekerField.Damage => 70,
                SeekerField.Interval => 90,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(MultiField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                MultiField.Damage => 60,
                MultiField.Interval => 80,
                MultiField.Count => 140,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(EmiField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                EmiField.Damage => 80,
                EmiField.Interval => 90,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(BeamField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                BeamField.Slow => 100,
                BeamField.Duration => 80,
                BeamField.Interval => 120,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(ShieldField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                ShieldField.Capacity => 120,
                ShieldField.Regen => 80,
                ShieldField.Startup => 60,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(SwordsField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                SwordsField.Damage => 70,
                SwordsField.Rate => 70,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(RunesField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                RunesField.Damage => 60,
                RunesField.Count => 130,
                RunesField.Speed => 80,
                RunesField.Interval => 90,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(BouncerField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                BouncerField.Damage => 80,
                BouncerField.Duration => 80,
                BouncerField.Interval => 100,
                BouncerField.Speed => 80,
                _ => 100
            };
            return basePrice * (lvl + 1);
        }

        public static int GetPrice(MortarField f)
        {
            int lvl = GetLevel(f);
            int basePrice = f switch
            {
                MortarField.Damage => 90,
                MortarField.Interval => 110,
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

        public static bool Buy(SeekerField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(MultiField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(EmiField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(BeamField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(ShieldField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(SwordsField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(RunesField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(BouncerField f)
        {
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(f);
            if (!coins.Spend(price)) return false;
            SetLevel(f, GetLevel(f) + 1);
            return true;
        }

        public static bool Buy(MortarField f)
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

        public static void ApplySeeker(TLS.Abilities.Seeker s)
        {
            if (s == null) return;
            int dmgL = GetLevel(SeekerField.Damage);
            int intL = GetLevel(SeekerField.Interval);
            s.damage = 5f + 2f * dmgL;
            s.interval = 0.5f * Mathf.Pow(0.9f, intL);
        }

        public static void ApplyMultishot(TLS.Abilities.Multishot m)
        {
            if (m == null) return;
            int dmgL = GetLevel(MultiField.Damage);
            int intL = GetLevel(MultiField.Interval);
            int cntL = GetLevel(MultiField.Count);
            m.damage = 3f + 1f * dmgL;
            m.interval = 3f * Mathf.Pow(0.9f, intL);
            m.count = 5 + cntL;
        }

        public static void ApplyAoEEMI(TLS.Abilities.AoEEMI a)
        {
            if (a == null) return;
            int dmgL = GetLevel(EmiField.Damage);
            int intL = GetLevel(EmiField.Interval);
            a.damage = 5f + 2f * dmgL;
            a.interval = 3f * Mathf.Pow(0.9f, intL);
        }

        public static void ApplyBeam(TLS.Abilities.BeamSlow b)
        {
            if (b == null) return;
            int slowL = GetLevel(BeamField.Slow);
            int durL = GetLevel(BeamField.Duration);
            int intL = GetLevel(BeamField.Interval);
            b.slowFactor = Mathf.Clamp01(0.4f - 0.03f * slowL); // stronger slow
            b.activeTime = 1.5f + 0.2f * durL;
            b.interval = 4f * Mathf.Pow(0.93f, intL);
        }

        public static void ApplyShield(TLS.Abilities.Shield s)
        {
            if (s == null) return;
            int capL = GetLevel(ShieldField.Capacity);
            int regL = GetLevel(ShieldField.Regen);
            int stL = GetLevel(ShieldField.Startup);
            s.capacity = Mathf.Max(1, 3 + capL);
            s.regenTime = 5f * Mathf.Pow(0.9f, regL);
            s.startupDelay = Mathf.Max(0f, 0f - 0.25f * stL); // could be 0; kept clamped
        }
    }
}
