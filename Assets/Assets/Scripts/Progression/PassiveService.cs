using UnityEngine;

namespace TLS.Progression
{
    // Manages purchased passives and simple pricing.
    public static class PassiveService
    {
        public enum Passive { CoinsPlus, Extra, Pierce, Crit, Spikes, TowerHP, Minute, Impatient, VeryImpatient }

        private static string Key(Passive p) => $"passive.{p}";
        public static bool Has(Passive p) => PlayerPrefs.GetInt(Key(p), 0) == 1;
        private static void Grant(Passive p) { PlayerPrefs.SetInt(Key(p), 1); PlayerPrefs.Save(); }

        public static int GetPrice(Passive p) => p switch
        {
            Passive.CoinsPlus => 500,
            Passive.Extra => 350,
            Passive.Pierce => 1500,
            Passive.Crit => 800,
            Passive.Spikes => 100,
            Passive.TowerHP => 300,
            Passive.Minute => 500,
            Passive.Impatient => 1500,
            Passive.VeryImpatient => 1500,
            _ => 1000
        };

        public static bool Buy(Passive p)
        {
            if (Has(p)) return false;
            if (p == Passive.VeryImpatient && !Has(Passive.Impatient)) return false;
            var coins = Coins.I; if (coins == null) return false;
            int price = GetPrice(p);
            if (!coins.Spend(price)) return false;
            Grant(p);
            TLS.Core.StatsService.IncPassivePurchase();
            return true;
        }

        // Helpers
        public static bool CoinsPlus => Has(Passive.CoinsPlus);
        public static bool Extra => Has(Passive.Extra);
        public static bool Pierce => Has(Passive.Pierce);
        public static bool Crit => Has(Passive.Crit);
        public static bool Spikes => Has(Passive.Spikes);
        public static bool Minute => Has(Passive.Minute);
        public static int TowerHPBonus => Has(Passive.TowerHP) ? 1 : 0;
        public static int StartMinute => Has(Passive.VeryImpatient) ? 2 : (Has(Passive.Impatient) ? 1 : 0);
    }
}

