using Unity.Mathematics;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;

    void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveToWardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }

    //eploxe sau 1 khoang time neu ko cham enemy
    public void SetupShard(float detinationTime)
    {
        Invoke(nameof(Explode), detinationTime);
    }

    private void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);

        Instantiate(vfxPrefab, transform.position, quaternion.identity);

        Destroy(gameObject);
    }

    
    //explode neu cham enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }

}
