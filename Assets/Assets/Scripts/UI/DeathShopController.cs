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
        [Header("Seeker Prices")]
        public Text priceSeekerDamage;
        public Text priceSeekerInterval;
        [Header("Multishot Prices")]
        public Text priceMultiDamage;
        public Text priceMultiInterval;
        public Text priceMultiCount;
        [Header("Swords Prices")]
        public Text priceSwordsDamage;
        public Text priceSwordsRate;
        [Header("Runes Prices")]
        public Text priceRunesDamage;
        public Text priceRunesCount;
        public Text priceRunesSpeed;
        public Text priceRunesInterval;
        [Header("EMI Prices")]
        public Text priceEmiDamage;
        public Text priceEmiInterval;
        [Header("Bouncer Prices")]
        public Text priceBouncerDamage;
        public Text priceBouncerDuration;
        public Text priceBouncerInterval;
        public Text priceBouncerSpeed;
        [Header("Mortar Prices")]
        public Text priceMortarDamage;
        public Text priceMortarInterval;
        [Header("Beam Prices")]
        public Text priceBeamSlow;
        public Text priceBeamDuration;
        public Text priceBeamInterval;
        [Header("Shield Prices")]
        public Text priceShieldCapacity;
        public Text priceShieldRegen;
        public Text priceShieldStartup;

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

        private bool _weaponSelected;

        private void OnEnable()
        {
            RefreshCoins();
            ShowUpgrades();
            InitializeWeaponSelection_New();
            // By default show only the weapon list; hide all upgrade panels until user selects a weapon
            HideAllUpgradePanels();
            if (_weaponSelected)
                RefreshWeaponPrices(selectedWeapon);
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
            if (u && _weaponSelected) RefreshWeaponPrices(selectedWeapon);
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

        private void RefreshSeekerPrices()
        {
            if (priceSeekerDamage) priceSeekerDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SeekerField.Damage).ToString();
            if (priceSeekerInterval) priceSeekerInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SeekerField.Interval).ToString();
        }

        private void RefreshMultishotPrices()
        {
            if (priceMultiDamage) priceMultiDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.MultiField.Damage).ToString();
            if (priceMultiInterval) priceMultiInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.MultiField.Interval).ToString();
            if (priceMultiCount) priceMultiCount.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.MultiField.Count).ToString();
        }

        private void RefreshSwordsPrices()
        {
            if (priceSwordsDamage) priceSwordsDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SwordsField.Damage).ToString();
            if (priceSwordsRate) priceSwordsRate.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.SwordsField.Rate).ToString();
        }

        private void RefreshRunesPrices()
        {
            if (priceRunesDamage) priceRunesDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.RunesField.Damage).ToString();
            if (priceRunesCount) priceRunesCount.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.RunesField.Count).ToString();
            if (priceRunesSpeed) priceRunesSpeed.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.RunesField.Speed).ToString();
            if (priceRunesInterval) priceRunesInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.RunesField.Interval).ToString();
        }

        private void RefreshEmiPrices()
        {
            if (priceEmiDamage) priceEmiDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.EmiField.Damage).ToString();
            if (priceEmiInterval) priceEmiInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.EmiField.Interval).ToString();
        }

        private void RefreshBouncerPrices()
        {
            if (priceBouncerDamage) priceBouncerDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BouncerField.Damage).ToString();
            if (priceBouncerDuration) priceBouncerDuration.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BouncerField.Duration).ToString();
            if (priceBouncerInterval) priceBouncerInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BouncerField.Interval).ToString();
            if (priceBouncerSpeed) priceBouncerSpeed.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BouncerField.Speed).ToString();
        }

        private void RefreshMortarPrices()
        {
            if (priceMortarDamage) priceMortarDamage.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.MortarField.Damage).ToString();
            if (priceMortarInterval) priceMortarInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.MortarField.Interval).ToString();
        }

        private void RefreshBeamPrices()
        {
            if (priceBeamSlow) priceBeamSlow.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BeamField.Slow).ToString();
            if (priceBeamDuration) priceBeamDuration.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BeamField.Duration).ToString();
            if (priceBeamInterval) priceBeamInterval.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.BeamField.Interval).ToString();
        }

        private void RefreshShieldPrices()
        {
            if (priceShieldCapacity) priceShieldCapacity.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.ShieldField.Capacity).ToString();
            if (priceShieldRegen) priceShieldRegen.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.ShieldField.Regen).ToString();
            if (priceShieldStartup) priceShieldStartup.text = TLS.Progression.UpgradeService.GetPrice(TLS.Progression.UpgradeService.ShieldField.Startup).ToString();
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

            // Clear other weapon price texts
            if (priceSeekerDamage) priceSeekerDamage.text = "";
            if (priceSeekerInterval) priceSeekerInterval.text = "";
            if (priceMultiDamage) priceMultiDamage.text = "";
            if (priceMultiInterval) priceMultiInterval.text = "";
            if (priceMultiCount) priceMultiCount.text = "";
            if (priceSwordsDamage) priceSwordsDamage.text = "";
            if (priceSwordsRate) priceSwordsRate.text = "";
            if (priceRunesDamage) priceRunesDamage.text = "";
            if (priceRunesCount) priceRunesCount.text = "";
            if (priceRunesSpeed) priceRunesSpeed.text = "";
            if (priceRunesInterval) priceRunesInterval.text = "";
            if (priceEmiDamage) priceEmiDamage.text = "";
            if (priceEmiInterval) priceEmiInterval.text = "";
            if (priceBouncerDamage) priceBouncerDamage.text = "";
            if (priceBouncerDuration) priceBouncerDuration.text = "";
            if (priceBouncerInterval) priceBouncerInterval.text = "";
            if (priceBouncerSpeed) priceBouncerSpeed.text = "";
            if (priceMortarDamage) priceMortarDamage.text = "";
            if (priceMortarInterval) priceMortarInterval.text = "";
            if (priceBeamSlow) priceBeamSlow.text = "";
            if (priceBeamDuration) priceBeamDuration.text = "";
            if (priceBeamInterval) priceBeamInterval.text = "";
            if (priceShieldCapacity) priceShieldCapacity.text = "";
            if (priceShieldRegen) priceShieldRegen.text = "";
            if (priceShieldStartup) priceShieldStartup.text = "";
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
            TMP_Text tmpText = buttonObj.GetComponentInChildren<TMP_Text>();

            // Set label text (supports Text or TMP_Text)
            if (tmpText != null) tmpText.text = weaponName;
            else if (buttonText != null) buttonText.text = weaponName;

            // Ensure the button stretches to container width
            var rt = buttonObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0f, rt.anchorMin.y);
                rt.anchorMax = new Vector2(1f, rt.anchorMax.y);
                rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
                rt.offsetMax = new Vector2(0f, rt.offsetMax.y);
            }

            if (button != null)
            {
                button.onClick.AddListener(() => SelectWeapon(weaponType));
            }
        }

        public void SelectWeapon(WeaponType weaponType)
        {
            _weaponSelected = true;
            selectedWeapon = weaponType;
            ShowWeaponUpgrades(weaponType);
            RefreshWeaponPrices(weaponType);
        }

        private void HideAllUpgradePanels()
        {
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
        }

        // New initializer with clean weapon names and auto layout setup
        private void InitializeWeaponSelection_New()
        {
            if (weaponButtonContainer == null || weaponButtonPrefab == null) return;

            // Auto-setup layout on container (VerticalLayout + ContentSizeFitter)
            var contGO = weaponButtonContainer.gameObject;
            var vlg = contGO.GetComponent<VerticalLayoutGroup>();
            if (vlg == null) vlg = contGO.AddComponent<VerticalLayoutGroup>();
            vlg.childControlWidth = true;
            vlg.childForceExpandWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 6f;
            vlg.padding = new RectOffset(6, 6, 6, 6);

            var fitter = contGO.GetComponent<ContentSizeFitter>();
            if (fitter == null) fitter = contGO.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Clear existing buttons
            foreach (Transform child in weaponButtonContainer)
            {
                Destroy(child.gameObject);
            }

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
                case WeaponType.Seeker:
                    RefreshSeekerPrices();
                    break;
                case WeaponType.Multishot:
                    RefreshMultishotPrices();
                    break;
                case WeaponType.Swords:
                    RefreshSwordsPrices();
                    break;
                case WeaponType.Runes:
                    RefreshRunesPrices();
                    break;
                case WeaponType.AoEEMI:
                    RefreshEmiPrices();
                    break;
                case WeaponType.Bouncer:
                    RefreshBouncerPrices();
                    break;
                case WeaponType.Mortar:
                    RefreshMortarPrices();
                    break;
                case WeaponType.Beam:
                    RefreshBeamPrices();
                    break;
                case WeaponType.Shield:
                    RefreshShieldPrices();
                    break;
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

        // Seeker upgrade buttons
        public void BuySeekerDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.SeekerField.Damage)) { ApplySeekerToAll(); RefreshCoins(); RefreshSeekerPrices(); } }
        public void BuySeekerInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.SeekerField.Interval)) { ApplySeekerToAll(); RefreshCoins(); RefreshSeekerPrices(); } }

        // Multishot upgrade buttons
        public void BuyMultiDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.MultiField.Damage)) { ApplyMultishotToAll(); RefreshCoins(); RefreshMultishotPrices(); } }
        public void BuyMultiInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.MultiField.Interval)) { ApplyMultishotToAll(); RefreshCoins(); RefreshMultishotPrices(); } }
        public void BuyMultiCount() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.MultiField.Count)) { ApplyMultishotToAll(); RefreshCoins(); RefreshMultishotPrices(); } }

        // AoE EMI upgrade buttons
        public void BuyEmiDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.EmiField.Damage)) { ApplyEmiToAll(); RefreshCoins(); RefreshEmiPrices(); } }
        public void BuyEmiInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.EmiField.Interval)) { ApplyEmiToAll(); RefreshCoins(); RefreshEmiPrices(); } }

        // Beam upgrade buttons
        public void BuyBeamSlow() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BeamField.Slow)) { ApplyBeamToAll(); RefreshCoins(); RefreshBeamPrices(); } }
        public void BuyBeamDuration() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BeamField.Duration)) { ApplyBeamToAll(); RefreshCoins(); RefreshBeamPrices(); } }
        public void BuyBeamInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BeamField.Interval)) { ApplyBeamToAll(); RefreshCoins(); RefreshBeamPrices(); } }

        // Shield upgrade buttons
        public void BuyShieldCapacity() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.ShieldField.Capacity)) { ApplyShieldToAll(); RefreshCoins(); RefreshShieldPrices(); } }
        public void BuyShieldRegen() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.ShieldField.Regen)) { ApplyShieldToAll(); RefreshCoins(); RefreshShieldPrices(); } }
        public void BuyShieldStartup() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.ShieldField.Startup)) { ApplyShieldToAll(); RefreshCoins(); RefreshShieldPrices(); } }

        // Placeholders: Swords/Runes/Bouncer/Mortar (apply when abilities added)
        public void BuySwordsDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.SwordsField.Damage)) { RefreshCoins(); RefreshSwordsPrices(); } }
        public void BuySwordsRate() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.SwordsField.Rate)) { RefreshCoins(); RefreshSwordsPrices(); } }
        public void BuyRunesDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.RunesField.Damage)) { RefreshCoins(); RefreshRunesPrices(); } }
        public void BuyRunesCount() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.RunesField.Count)) { RefreshCoins(); RefreshRunesPrices(); } }
        public void BuyRunesSpeed() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.RunesField.Speed)) { RefreshCoins(); RefreshRunesPrices(); } }
        public void BuyRunesInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.RunesField.Interval)) { RefreshCoins(); RefreshRunesPrices(); } }
        public void BuyBouncerDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BouncerField.Damage)) { RefreshCoins(); RefreshBouncerPrices(); } }
        public void BuyBouncerDuration() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BouncerField.Duration)) { RefreshCoins(); RefreshBouncerPrices(); } }
        public void BuyBouncerInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BouncerField.Interval)) { RefreshCoins(); RefreshBouncerPrices(); } }
        public void BuyBouncerSpeed() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.BouncerField.Speed)) { RefreshCoins(); RefreshBouncerPrices(); } }
        public void BuyMortarDamage() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.MortarField.Damage)) { RefreshCoins(); RefreshMortarPrices(); } }
        public void BuyMortarInterval() { if (TLS.Progression.UpgradeService.Buy(TLS.Progression.UpgradeService.MortarField.Interval)) { RefreshCoins(); RefreshMortarPrices(); } }

        private void ApplySeekerToAll()
        {
            foreach (var s in GameObject.FindObjectsOfType<TLS.Abilities.Seeker>())
                TLS.Progression.UpgradeService.ApplySeeker(s);
        }

        private void ApplyMultishotToAll()
        {
            foreach (var m in GameObject.FindObjectsOfType<TLS.Abilities.Multishot>())
                TLS.Progression.UpgradeService.ApplyMultishot(m);
        }

        private void ApplyEmiToAll()
        {
            foreach (var e in GameObject.FindObjectsOfType<TLS.Abilities.AoEEMI>())
                TLS.Progression.UpgradeService.ApplyAoEEMI(e);
        }

        private void ApplyBeamToAll()
        {
            foreach (var b in GameObject.FindObjectsOfType<TLS.Abilities.BeamSlow>())
                TLS.Progression.UpgradeService.ApplyBeam(b);
        }

        private void ApplyShieldToAll()
        {
            foreach (var s in GameObject.FindObjectsOfType<TLS.Abilities.Shield>())
                TLS.Progression.UpgradeService.ApplyShield(s);
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

        // Duplicate legacy initializer (disabled)
        private void InitializeWeaponSelection_New_Legacy()
        {
            if (weaponButtonContainer == null || weaponButtonPrefab == null) return;

            // Clear existing buttons
            foreach (Transform child in weaponButtonContainer)
            {
                Destroy(child.gameObject);
            }

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
    }
}
