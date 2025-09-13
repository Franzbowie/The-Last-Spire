using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State { Menu, Run, DeathShop }
    public static GameManager I;
    public State Current;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        StartRun();
    }

    public void StartRun()
    {
        Current = State.Run;
        Time.timeScale = 1f;
    }

    public void OnTowerDead()
    {
        Current = State.DeathShop;
        Time.timeScale = 0f;
        // TODO: show death shop UI
    }
}

