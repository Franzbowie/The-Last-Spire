using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TLS.UI
{
    public enum WeaponType
    {
        SkyStrike,    // Небесный снаряд
        Seeker,       // Искатель
        Multishot,    // Мультивыстрел
        Swords,       // Мечи
        Runes,        // Руны
        AoEEMI,       // ЭМИ
        Bouncer,      // Вышибала
        Mortar,       // Мортира
        Beam,         // Луч
        Shield        // Щит
    }

    // Minimal controller for DeathShop: tabs + simple purchases using Coins.
    public class DeathShopController : MonoBehaviour
    {
        [Header("Panels")] public GameObject upgradesTab; // assign panel GameObjects
        public GameObject passivesTab;
        public GameObject statsTab;

        [Header("Tab Buttons")] public UnityEngine.UI.Button upgradesButton;
        public UnityEngine.UI.Button passivesButton;
        public UnityEngine.UI.Button statsButton;

        [Header("Wallet")] public TextMeshProUGUI coinsText;

        [Header("Weapon Selection")]
        public ScrollRect weaponScrollRect;
        public Transform weaponButtonContainer;
        public GameObject weaponButtonPrefab;
        private WeaponType selectedWeapon = WeaponType.SkyStrike;

        [Header("Upgrade Panels")]
        public GameObject skyStrikeUpgrades;
        public GameObject seekerUpgrades;
        public GameObject multishotUpgrades;
        public GameObject swordsUpgrades;
        public GameObject runesUpgrades;
        public GameObject aoEEMIUpgrades;
        public GameObject bouncerUpgrades;
        public GameObject mortarUpgrades;
        public GameObject beamUpgrades;
        public GameObject shieldUpgrades;

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
            InitializeWeaponSelection();
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
            // Clear all price texts first to avoid text persistence
            ClearAllPriceTexts();
            
            // Show/hide panels
            if (upgradesTab) upgradesTab.SetActive(u);
            if (passivesTab) passivesTab.SetActive(p);
            if (statsTab) statsTab.SetActive(s);
            
            // Show/hide coins text based on active tab
            // Show coins in Upgrades and Passives tabs, hide in Stats tab
            bool shouldShowCoins = u || p;
            if (coinsText != null) coinsText.gameObject.SetActive(shouldShowCoins);
            
            // Update button visual states
            UpdateTabButtonStates(u, p, s);
            
            RefreshCoins();
            if (u) RefreshUpgradePrices();
            if (p) RefreshPassivePrices();
            if (s) RefreshStats();
        }

        private void UpdateTabButtonStates(bool upgradesActive, bool passivesActive, bool statsActive)
        {
            // Update button colors to show which tab is active
            if (upgradesButton != null)
            {
                var colors = upgradesButton.colors;
                colors.normalColor = upgradesActive ? Color.white : Color.gray;
                upgradesButton.colors = colors;
            }
            
            if (passivesButton != null)
            {
                var colors = passivesButton.colors;
                colors.normalColor = passivesActive ? Color.white : Color.gray;
                passivesButton.colors = colors;
            }
            
            if (statsButton != null)
            {
                var colors = statsButton.colors;
                colors.normalColor = statsActive ? Color.white : Color.gray;
                statsButton.colors = colors;
            }
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

        public void RefreshCoins()
        {
            if (coinsText != null && TLS.Progression.Coins.I != null)
            {
                coinsText.text = TLS.Progression.Coins.I.total.ToString() + " Монет";
            }
            else
            {
                if (coinsText == null)
                    Debug.LogWarning("DeathShopController: coinsText not assigned in Inspector!");
                if (TLS.Progression.Coins.I == null)
                    Debug.LogWarning("DeathShopController: Coins service not found!");
            }
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

        private void RefreshStats()
        {
            // Find and refresh StatsView component
            var statsView = statsTab?.GetComponentInChildren<StatsView>();
            if (statsView != null)
            {
                statsView.Refresh();
            }
        }

        private void ClearAllPriceTexts()
        {
            // Clear upgrade price texts
            if (priceSkyDamage) priceSkyDamage.text = "";
            if (priceSkyRadius) priceSkyRadius.text = "";
            if (priceSkyInterval) priceSkyInterval.text = "";
            if (priceSkyCount) priceSkyCount.text = "";

            // Clear passive price texts
            if (priceCoinsPlus) priceCoinsPlus.text = "";
            if (priceExtra) priceExtra.text = "";
            if (pricePierce) pricePierce.text = "";
            if (priceCrit) priceCrit.text = "";
            if (priceSpikes) priceSpikes.text = "";
            if (priceTowerHP) priceTowerHP.text = "";
            if (priceMinute) priceMinute.text = "";
            if (priceImpatient) priceImpatient.text = "";
            if (priceVeryImpatient) priceVeryImpatient.text = "";
        }

        // Weapon selection methods
        private void InitializeWeaponSelection()
        {
            if (weaponButtonContainer == null || weaponButtonPrefab == null) return;

            // Clear existing buttons
            foreach (Transform child in weaponButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // Create buttons for each weapon type
            string[] weaponNames = {
                "Небесный снаряд", "Искатель", "Мультивыстрел", "Мечи", "Руны",
                "ЭМИ", "Вышибала", "Мортира", "Луч", "Щит"
            };

            for (int i = 0; i < weaponNames.Length; i++)
            {
                WeaponType weaponType = (WeaponType)i;
                CreateWeaponButton(weaponType, weaponNames[i]);
            }

            // Select default weapon
            SelectWeapon(selectedWeapon);
        }

        private void CreateWeaponButton(WeaponType weaponType, string weaponName)
        {
            GameObject buttonObj = Instantiate(weaponButtonPrefab, weaponButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            
            if (buttonText != null)
                buttonText.text = weaponName;

            if (button != null)
            {
                button.onClick.AddListener(() => SelectWeapon(weaponType));
            }
        }

        public void SelectWeapon(WeaponType weaponType)
        {
            selectedWeapon = weaponType;
            ShowWeaponUpgrades(weaponType);
            RefreshWeaponPrices(weaponType);
        }

        private void ShowWeaponUpgrades(WeaponType weaponType)
        {
            // Hide all upgrade panels
            if (skyStrikeUpgrades) skyStrikeUpgrades.SetActive(false);
            if (seekerUpgrades) seekerUpgrades.SetActive(false);
            if (multishotUpgrades) multishotUpgrades.SetActive(false);
            if (swordsUpgrades) swordsUpgrades.SetActive(false);
            if (runesUpgrades) runesUpgrades.SetActive(false);
            if (aoEEMIUpgrades) aoEEMIUpgrades.SetActive(false);
            if (bouncerUpgrades) bouncerUpgrades.SetActive(false);
            if (mortarUpgrades) mortarUpgrades.SetActive(false);
            if (beamUpgrades) beamUpgrades.SetActive(false);
            if (shieldUpgrades) shieldUpgrades.SetActive(false);

            // Show selected weapon's upgrade panel
            GameObject selectedPanel = GetWeaponUpgradePanel(weaponType);
            if (selectedPanel != null)
                selectedPanel.SetActive(true);
        }

        private GameObject GetWeaponUpgradePanel(WeaponType weaponType)
        {
            return weaponType switch
            {
                WeaponType.SkyStrike => skyStrikeUpgrades,
                WeaponType.Seeker => seekerUpgrades,
                WeaponType.Multishot => multishotUpgrades,
                WeaponType.Swords => swordsUpgrades,
                WeaponType.Runes => runesUpgrades,
                WeaponType.AoEEMI => aoEEMIUpgrades,
                WeaponType.Bouncer => bouncerUpgrades,
                WeaponType.Mortar => mortarUpgrades,
                WeaponType.Beam => beamUpgrades,
                WeaponType.Shield => shieldUpgrades,
                _ => null
            };
        }

        private void RefreshWeaponPrices(WeaponType weaponType)
        {
            // Clear all prices first
            ClearAllPriceTexts();

            // Refresh prices for selected weapon
            switch (weaponType)
            {
                case WeaponType.SkyStrike:
                    RefreshUpgradePrices();
                    break;
                // Add other weapon price refresh methods here as they are implemented
                default:
                    // For now, only SkyStrike has implemented upgrades
                    break;
            }
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
            // Hide death shop and start new run immediately
            gameObject.SetActive(false);
            
            // Reset game state and start new run
            if (TLS.Core.GameManager.I != null)
            {
                TLS.Core.GameManager.I.StartRun();
            }
        }
    }
}
