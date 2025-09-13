using UnityEngine;

namespace TLS.Progression
{
    public class Coins : MonoBehaviour
    {
        public static Coins I;

        public int total { get; private set; }

        private void Awake()
        {
            if (I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }
            I = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }

        public void Add(int n)
        {
            if (n <= 0) return;
            total += n;
            TLS.Core.StatsService.AddCoinsEarned(n);
            Save();
            TLS.UI.UI.HUD.SetCoins(total);
        }

        public bool Spend(int price)
        {
            if (price <= 0) return false;
            if (total < price) return false;
            total -= price;
            Save();
            TLS.UI.UI.HUD.SetCoins(total);
            return true;
        }

        public void ResetAll()
        {
            total = 0;
            Save();
            TLS.UI.UI.HUD.SetCoins(total);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("coins", total);
        }

        private void Load()
        {
            total = PlayerPrefs.GetInt("coins", 0);
        }
    }
}
