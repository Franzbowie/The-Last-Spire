using UnityEngine;
using TMPro;

namespace TLS.UI
{
    // Bind this to a Canvas object; assign Text fields in inspector
    public class HUDController : MonoBehaviour
    {
        [Header("TMP Text references")]
        public TMP_Text coinsText;
        public TMP_Text timeText;
        public TMP_Text killsText;

        private void Awake()
        {
            UI.HUD.onSetCoins += UpdateCoins;
            UI.HUD.onSetTime += UpdateTime;
            UI.HUD.onSetKills += UpdateKills;
        }

        private void OnDestroy()
        {
            UI.HUD.onSetCoins -= UpdateCoins;
            UI.HUD.onSetTime -= UpdateTime;
            UI.HUD.onSetKills -= UpdateKills;
        }

        private void UpdateCoins(int total)
        {
            if (coinsText != null)
                coinsText.text = total.ToString();
        }

        private void UpdateTime(float seconds)
        {
            if (timeText != null)
                timeText.text = FormatTime(seconds);
        }

        private void UpdateKills(int kills)
        {
            if (killsText != null)
                killsText.text = kills.ToString();
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
