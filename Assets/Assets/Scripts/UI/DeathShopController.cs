using UnityEngine;
using UnityEngine.UI;

namespace TLS.UI
{
    // Minimal controller for DeathShop: tabs + simple purchases using Coins.
    public class DeathShopController : MonoBehaviour
    {
        [Header("Panels")] public GameObject upgradesTab; // assign panel GameObjects
        public GameObject passivesTab;
        public GameObject statsTab;

        [Header("Wallet")] public Text coinsText;

        [Header("SkyStrike Prices")]
        public Text priceSkyDamage;
        public Text priceSkyRadius;
        public Text priceSkyInterval;
        public Text priceSkyCount;

        [Header("Passives Prices")]
        public Text priceCoinsPlus;
        public Text priceExtra;
        public Text pricePierce;
        public Text priceCrit;
        public Text priceSpikes;
        public Text priceTowerHP;
        public Text priceMinute;
        public Text priceImpatient;
        public Text priceVeryImpatient;

        private void OnEnable()
        {
            RefreshCoins();
            ShowUpgrades();
            RefreshUpgradePrices();
        }

        public void ShowUpgrades()
        {
            SetTabs(true, false, false);
        }

        public void ShowPassives()
        {
            SetTabs(false, true, false);
        }

        public void ShowStats()
        {
            SetTabs(false, false, true);
        }

        private void SetTabs(bool u, bool p, bool s)
        {
            if (upgradesTab) upgradesTab.SetActive(u);
            if (passivesTab) passivesTab.SetActive(p);
            if (statsTab) statsTab.SetActive(s);
            RefreshCoins();
            if (u) RefreshUpgradePrices();
            if (p) RefreshPassivePrices();
        }

        public void Purchase(int price, string kind)
        {
            var coins = TLS.Progression.Coins.I;
            if (coins == null) { Debug.LogWarning("Coins service missing"); return; }
            if (!coins.Spend(price)) { Debug.Log("Not enough coins"); return; }
            if (kind == "passive") TLS.Core.StatsService.IncPassivePurchase();
            RefreshCoins();
            Debug.Log($"Purchased {kind} for {price}");
        }

        private void RefreshCoins()
        {
            if (coinsText != null && TLS.Progression.Coins.I != null)
                coinsText.text = TLS.Progression.Coins.I.total.ToString();
        }

        private void RefreshUpgradePrices()
        {
            if (priceSkyDamage) priceSkyDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SkyField.Damage).ToString();
            if (priceSkyRadius) priceSkyRadius.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SkyField.Radius).ToString();
            if (priceSkyInterval) priceSkyInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SkyField.Interval).ToString();
            if (priceSkyCount) priceSkyCount.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SkyField.Count).ToString();
        }

        private void RefreshPassivePrices()
        {
            if (priceCoinsPlus) priceCoinsPlus.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.CoinsPlus).ToString();
            if (priceExtra) priceExtra.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Extra).ToString();
            if (pricePierce) pricePierce.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Pierce).ToString();
            if (priceCrit) priceCrit.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Crit).ToString();
            if (priceSpikes) priceSpikes.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Spikes).ToString();
            if (priceTowerHP) priceTowerHP.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.TowerHP).ToString();
            if (priceMinute) priceMinute.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Minute).ToString();
            if (priceImpatient) priceImpatient.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.Impatient).ToString();
            if (priceVeryImpatient) priceVeryImpatient.text = TLS.Progression.PassiveService.GetPrice(TLS.Progression.PassiveService.Passive.VeryImpatient).ToString();
        }

        // Upgrade buttons
        public void BuySkyDamage() { BuySky(TLS.Progression.UpgradeService.SkyField.Damage); }
        public void BuySkyRadius() { BuySky(TLS.Progression.UpgradeService.SkyField.Radius); }
        public void BuySkyInterval() { BuySky(TLS.Progression.UpgradeService.SkyField.Interval); }
        public void BuySkyCount() { BuySky(TLS.Progression.UpgradeService.SkyField.Count); }

        private void BuySky(TLS.Progression.UpgradeService.SkyField field)
        {
            if (TLS.Progression.UpgradeService.Buy(field))
            {
                ApplySkyToAll();
                RefreshCoins();
                RefreshUpgradePrices();
            }
            else Debug.Log("Not enough coins or upgrade invalid");
        }

        private void ApplySkyToAll()
        {
            foreach (var sky in GameObject.FindObjectsOfType<TLS.Abilities.SkyStrike>())
                TLS.Progression.UpgradeService.ApplySkyStrike(sky);
        }

        // Passive buttons
        public void BuyPassive_CoinsPlus() { BuyPassive(TLS.Progression.PassiveService.Passive.CoinsPlus); }
        public void BuyPassive_Extra() { BuyPassive(TLS.Progression.PassiveService.Passive.Extra); }
        public void BuyPassive_Pierce() { BuyPassive(TLS.Progression.PassiveService.Passive.Pierce); }
        public void BuyPassive_Crit() { BuyPassive(TLS.Progression.PassiveService.Passive.Crit); }
        public void BuyPassive_Spikes() { BuyPassive(TLS.Progression.PassiveService.Passive.Spikes); }
        public void BuyPassive_TowerHP() { BuyPassive(TLS.Progression.PassiveService.Passive.TowerHP); }
        public void BuyPassive_Minute() { BuyPassive(TLS.Progression.PassiveService.Passive.Minute); }
        public void BuyPassive_Impatient() { BuyPassive(TLS.Progression.PassiveService.Passive.Impatient); }
        public void BuyPassive_VeryImpatient() { BuyPassive(TLS.Progression.PassiveService.Passive.VeryImpatient); }

        private void BuyPassive(TLS.Progression.PassiveService.Passive p)
        {
            if (TLS.Progression.PassiveService.Buy(p))
            {
                RefreshCoins();
                RefreshPassivePrices();
            }
            else Debug.Log("Passive buy failed or already owned");
        }

        // UI buttons
        public void OnClickRestart()
        {
            TLS.Core.RunStarter.SetAutoStartNextScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void OnClickContinue()
        {
            // Go to menu (reload without autostart)
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
