using UnityEngine;

namespace TLS.UI
{
    // Minimal stubs so gameplay compiles without UI implemented yet
    public static class UI
    {
        public static void ShowDeathShop()
        {
            // Try to enable a DeathShop panel if present in scene
            var shop = Object.FindObjectOfType<DeathShopController>(includeInactive: true);
            if (shop != null)
            {
                shop.gameObject.SetActive(true);
                // Force refresh coins when shop opens
                shop.RefreshCoins();
            }
            else
            {
                Debug.Log("DeathShop opened (stub)");
            }
        }

        public static class HUD
        {
            public static System.Action<int> onSetCoins;
            public static System.Action<float> onSetTime;
            public static System.Action<int> onSetKills;

            public static void SetCoins(int total)
            {
                if (onSetCoins != null) onSetCoins(total);
                else Debug.Log($"Coins: {total}");
            }

            public static void SetTime(float seconds)
            {
                if (onSetTime != null) onSetTime(seconds);
                else Debug.Log($"Time: {seconds:0.0}s");
            }

            public static void SetKills(int kills)
            {
                if (onSetKills != null) onSetKills(kills);
                else Debug.Log($"Kills: {kills}");
            }
        }

        public static class Toasts
        {
            public static void Show(string message)
            {
                Debug.Log($"Toast: {message}");
            }
        }
    }
}
