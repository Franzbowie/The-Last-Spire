using UnityEngine;

namespace TLS.Progression
{
    public static class UnlocksService
    {
        public enum Ability { Seeker, Multishot, Runes, AoEEMI, Bouncer, Mortar, Beam, Shield }

        private static string Key(Ability a) => $"unlocks.{a}";

        public static bool IsUnlocked(Ability a) => PlayerPrefs.GetInt(Key(a), 0) == 1;
        private static void Unlock(Ability a)
        {
            if (IsUnlocked(a)) return;
            PlayerPrefs.SetInt(Key(a), 1);
            PlayerPrefs.Save();
            Debug.Log($"Unlocked: {a}");
        }

        public static void CheckAndUnlockAll()
        {
            // Map conditions from GDD
            if (!IsUnlocked(Ability.Seeker) && TLS.Core.StatsService.TotalRuns >= 4) Unlock(Ability.Seeker);
            if (!IsUnlocked(Ability.Multishot) && TLS.Core.StatsService.TotalKills >= 100) Unlock(Ability.Multishot);
            if (!IsUnlocked(Ability.Runes) && TLS.Core.StatsService.LongestRun >= 120f) Unlock(Ability.Runes);
            if (!IsUnlocked(Ability.AoEEMI) && TLS.Core.StatsService.CoinsEarned >= 2000) Unlock(Ability.AoEEMI);
            if (!IsUnlocked(Ability.Bouncer) && TLS.Core.StatsService.PassivePurchases >= 3) Unlock(Ability.Bouncer);
            // Mortar: max out any one weapon (TBD) — skip for now
            if (!IsUnlocked(Ability.Beam) && TLS.Core.StatsService.TotalPlayTime >= 30f * 60f) Unlock(Ability.Beam);
            if (!IsUnlocked(Ability.Shield) && TLS.Core.StatsService.TotalKills >= 2000) Unlock(Ability.Shield);
        }
    }
}

