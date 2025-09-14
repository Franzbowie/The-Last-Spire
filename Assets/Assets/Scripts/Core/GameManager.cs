using UnityEngine;
using System;
using System.Reflection;

namespace TLS.Core
{
    public class GameManager : MonoBehaviour
    {
        public enum State { Menu, Run, DeathShop }

        public static GameManager I;
        public static event Action<State> OnStateChanged;

        public State Current { get; private set; } = State.Menu;

        private void Awake()
        {
            if (I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }
            I = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartRun()
        {
            // Clean up previous run
            CleanupPreviousRun();
            
            SetState(State.Run);
            Time.timeScale = 1f;
            TLS.Core.StatsService.OnRunStart();
        }

        private void CleanupPreviousRun()
        {
            // Destroy all enemies
            var enemies = GameObject.FindObjectsOfType<TLS.Combat.EnemyController>();
            foreach (var enemy in enemies)
            {
                if (enemy != null) Destroy(enemy.gameObject);
            }

            // Destroy all projectiles
            var projectiles = GameObject.FindObjectsOfType<TLS.Projectiles.Projectile>();
            foreach (var proj in projectiles)
            {
                if (proj != null) Destroy(proj.gameObject);
            }

            // Destroy sky strike projectiles
            var skyProjectiles = GameObject.FindObjectsOfType<TLS.Projectiles.SkyStrikeProjectile>();
            foreach (var skyProj in skyProjectiles)
            {
                if (skyProj != null) Destroy(skyProj.gameObject);
            }

            // Reset tower health
            var towerHealth = FindObjectOfType<TLS.Combat.TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.segments = 1 + TLS.Progression.PassiveService.TowerHPBonus;
            }

            // Reset shield if present
            var shield = FindObjectOfType<TLS.Abilities.Shield>();
            if (shield != null)
            {
                // Reset shield to full capacity
                var shieldComponent = shield.GetComponent<TLS.Abilities.Shield>();
                if (shieldComponent != null)
                {
                    // Use reflection to reset private fields
                    var field = typeof(TLS.Abilities.Shield).GetField("_cur", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (field != null) field.SetValue(shieldComponent, shieldComponent.capacity);
                }
            }
        }

        public void OnTowerDead()
        {
            SetState(State.DeathShop);
            Time.timeScale = 0f;
            TLS.Core.StatsService.OnRunEnd();
            TLS.UI.UI.ShowDeathShop();
        }

        private void SetState(State newState)
        {
            if (Current != newState)
            {
                Current = newState;
                OnStateChanged?.Invoke(newState);
            }
        }

        // Clears all PlayerPrefs-backed saves (coins, stats, upgrades, passives, unlocks)
        public static void ClearAllSaves()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            // Reset runtime mirrors if services are alive
            if (TLS.Progression.Coins.I != null)
            {
                TLS.Progression.Coins.I.ResetAll();
            }
            TLS.UI.UI.Toasts.Show("Сохранения очищены");
        }

#if UNITY_EDITOR
        [ContextMenu("Clear Saves (PlayerPrefs)")]
        private void ClearSavesContext()
        {
            ClearAllSaves();
        }
#endif
    }
}
