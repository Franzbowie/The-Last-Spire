using UnityEngine;
using UnityEngine.UI;

namespace TLS.UI
{
    // Simple binder to show profile stats on UI
    public class StatsView : MonoBehaviour
    {
        public Text totalKills;
        public Text longestRun;
        public Text totalPlay;
        public Text totalRuns;
        public Text coinsEarned;

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (totalKills) totalKills.text = TLS.Core.StatsService.TotalKills.ToString();
            if (longestRun) longestRun.text = FormatTime(TLS.Core.StatsService.LongestRun);
            if (totalPlay) totalPlay.text = FormatTime(TLS.Core.StatsService.TotalPlayTime);
            if (totalRuns) totalRuns.text = TLS.Core.StatsService.TotalRuns.ToString();
            if (coinsEarned) coinsEarned.text = TLS.Core.StatsService.CoinsEarned.ToString();
        }

        private static string FormatTime(float sec)
        {
            if (sec < 0f) sec = 0f;
            int s = Mathf.FloorToInt(sec % 60f);
            int m = Mathf.FloorToInt(sec / 60f);
            return $"{m:00}:{s:00}";
        }
    }
}

