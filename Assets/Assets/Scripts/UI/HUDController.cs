using UnityEngine;
using TMPro;
using TLS.Core;

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
            
            // Hide HUD elements initially
            SetHUDVisibility(false);
        }

        private void Start()
        {
            // Subscribe to GameManager state changes
            GameManager.OnStateChanged += OnGameStateChanged;
            
            // Check current state and show/hide accordingly
            if (GameManager.I != null)
            {
                UpdateHUDForGameState(GameManager.I.Current);
            }
        }

        private void OnDestroy()
        {
            UI.HUD.onSetCoins -= UpdateCoins;
            UI.HUD.onSetTime -= UpdateTime;
            UI.HUD.onSetKills -= UpdateKills;
            GameManager.OnStateChanged -= OnGameStateChanged;
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

        private void SetHUDVisibility(bool visible)
        {
            if (coinsText != null) coinsText.gameObject.SetActive(visible);
            if (timeText != null) timeText.gameObject.SetActive(visible);
            if (killsText != null) killsText.gameObject.SetActive(visible);
        }

        private void UpdateHUDForGameState(GameManager.State state)
        {
            bool shouldShowHUD = state == GameManager.State.Run;
            SetHUDVisibility(shouldShowHUD);
        }

        // Public method to be called when game state changes
        public void OnGameStateChanged(GameManager.State newState)
        {
            UpdateHUDForGameState(newState);
        }
    }
}
