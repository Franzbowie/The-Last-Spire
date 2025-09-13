using UnityEngine;

public class Coins : MonoBehaviour
{
    public static Coins I;
    public int total;

    void Awake()
    {
        I = this;
        total = PlayerPrefs.GetInt("coins", 0);
    }

    public void Add(int n)
    {
        total += n;
        PlayerPrefs.SetInt("coins", total);
        // TODO: update HUD
    }
}

