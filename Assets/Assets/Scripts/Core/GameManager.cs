using UnityEngine;

namespace TLS.Core
{
    public class GameManager : MonoBehaviour
    {
        public enum State { Menu, Run, DeathShop }

        public static GameManager I;

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
            Current = State.Run;
            Time.timeScale = 1f;
            TLS.Core.StatsService.OnRunStart();
        }

        public void OnTowerDead()
        {
            Current = State.DeathShop;
            Time.timeScale = 0f;
            TLS.Core.StatsService.OnRunEnd();
            TLS.UI.UI.ShowDeathShop();
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
