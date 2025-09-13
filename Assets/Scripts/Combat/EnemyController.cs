using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float hp = 10f;
    public float speed = 1f;
    public float dmg = 1f;
    private Transform tower;

    void Start()
    {
        var th = FindObjectOfType<TowerHealth>();
        if (th != null)
            tower = th.transform;
    }

    void Update()
    {
        if (tower == null) return;
        transform.position = Vector3.MoveTowards(transform.position, tower.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TowerHealth th))
        {
            th.TakeHit((int)dmg);
            Die(false);
        }
    }

    public void Die(bool killedByPlayer)
    {
        if (killedByPlayer)
        {
            Coins.I.Add(5);
        }
        Destroy(gameObject);
    }
}

