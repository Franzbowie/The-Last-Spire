using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public int segments = 1;

    public void TakeHit(int dmg = 1)
    {
        segments -= dmg;
        if (segments <= 0)
        {
            GameManager.I.OnTowerDead();
        }
    }
}

