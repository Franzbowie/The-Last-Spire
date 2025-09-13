using System.Collections;
using UnityEngine;

public class SkyStrike : MonoBehaviour
{
    public float interval = 0.5f;
    public float delay = 0.25f;
    public float radius = 1f;
    public float damage = 10f;

    float timer;
    AimProvider aim;

    void Start()
    {
        aim = FindObjectOfType<AimProvider>();
    }

    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = interval;
            StartCoroutine(Strike(aim.AimPoint));
        }
#else
        if (Input.touchCount > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = interval;
                StartCoroutine(Strike(aim.AimPoint));
            }
        }
#endif
    }

    IEnumerator Strike(Vector3 p)
    {
        yield return new WaitForSeconds(delay);
        foreach (var hit in Physics2D.OverlapCircleAll(p, radius))
        {
            if (hit.TryGetComponent(out EnemyController e))
            {
                e.hp -= damage;
                if (e.hp <= 0)
                    e.Die(true);
            }
        }
    }
}

