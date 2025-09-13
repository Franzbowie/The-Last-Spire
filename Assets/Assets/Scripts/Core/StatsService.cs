using UnityEngine;

namespace TLS.Core
{
    // Tracks kills, run time, longest run, total play time and updates HUD time.
    public class StatsService : MonoBehaviour
    {
        private static StatsService _inst;

        public static int TotalKills => PlayerPrefs.GetInt("stats.totalKills", 0);
        public static float LongestRun => PlayerPrefs.GetFloat("stats.longestRun", 0f);
        public static float TotalPlayTime => PlayerPrefs.GetFloat("stats.totalPlay", 0f);
        public static int TotalRuns => PlayerPrefs.GetInt("stats.totalRuns", 0);
        public static int PassivePurchases => PlayerPrefs.GetInt("stats.passivePurchases", 0);
        public static int CoinsEarned => PlayerPrefs.GetInt("stats.coinsEarned", 0);

        private bool _inRun;
        private float _runTime;
        private float _hudTimer;
        private int _runKills;
        private float _minuteTimer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (_inst == null)
            {
                var go = new GameObject("StatsService");
                _inst = go.AddComponent<StatsService>();
                DontDestroyOnLoad(go);
            }
        }

        private void Update()
        {
            if (_inRun)
            {
                float dt = Time.unscaledDeltaTime; // track even if paused logic-wise
                _runTime += Time.deltaTime;        // gameplay time
                _hudTimer += dt;
                if (TLS.Progression.PassiveService.Minute)
                {
                    _minuteTimer += Time.deltaTime;
                    if (_minuteTimer >= 60f)
                    {
                        _minuteTimer -= 60f;
                        TLS.Progression.Coins.I?.Add(100);
                    }
                }
                if (_hudTimer >= 0.1f)
                {
                    _hudTimer = 0f;
                    TLS.UI.UI.HUD.SetTime(_runTime);
                }
            }
        }

        public static void OnRunStart()
        {
            Ensure();
            _inst._inRun = true;
            _inst._runTime = 0f;
            _inst._hudTimer = 0f;
            _inst._runKills = 0;
            _inst._minuteTimer = 0f;
            TLS.UI.UI.HUD.SetTime(0f);
            TLS.UI.UI.HUD.SetKills(0);
        }

        public static void OnRunEnd()
        {
            Ensure();
            _inst._inRun = false;
            float longest = LongestRun;
            if (_inst._runTime > longest)
            {
                PlayerPrefs.SetFloat("stats.longestRun", _inst._runTime);
            }
            PlayerPrefs.SetFloat("stats.totalPlay", TotalPlayTime + _inst._runTime);
            PlayerPrefs.SetInt("stats.totalRuns", TotalRuns + 1);
            PlayerPrefs.Save();

            TLS.Progression.UnlocksService.CheckAndUnlockAll();

            // Milestone 5:00
            if (_inst._runTime >= 300f && PlayerPrefs.GetInt("milestone.5.first", 0) == 0)
            {
                PlayerPrefs.SetInt("milestone.5.first", 1);
                PlayerPrefs.Save();
                TLS.UI.UI.Toasts.Show("Поздравляем! Достигнуто 5:00. Авто-реплей.");
                TLS.Core.RunStarter.SetAutoStartNextScene();
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }

        public static void IncKill()
        {
            PlayerPrefs.SetInt("stats.totalKills", TotalKills + 1);
            Ensure();
            _inst._runKills++;
            TLS.UI.UI.HUD.SetKills(_inst._runKills);
            PlayerPrefs.Save();
        }

        public static void AddCoinsEarned(int n)
        {
            if (n <= 0) return;
            PlayerPrefs.SetInt("stats.coinsEarned", CoinsEarned + n);
            PlayerPrefs.Save();
        }

        public static void IncPassivePurchase()
        {
            PlayerPrefs.SetInt("stats.passivePurchases", PassivePurchases + 1);
            PlayerPrefs.Save();
        }

        private static void Ensure()
        {
            if (_inst == null)
            {
                var go = new GameObject("StatsService");
                _inst = go.AddComponent<StatsService>();
                DontDestroyOnLoad(go);
            }
        }
    }
}
