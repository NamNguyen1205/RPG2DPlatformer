using System;
using Unity.Mathematics;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;

    private Skill_Shard shardManager;

    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;

    void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveToWardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget == null ? FindClosestTarget() : newTarget;
        this.speed = speed;
    }

    //eploxe sau 1 khoang time neu ko cham enemy
    public void SetupShard(Skill_Shard shardManager)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;

        float detonationTime = shardManager.GetDetonateTime();


        Invoke(nameof(Explode), detonationTime);
    }

    public void SetupShard(Skill_Shard shardManager, float detonationTime, bool canMove, float shardSpeed, Transform target = null)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;


        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveToWardsClosestTarget(shardSpeed, target);
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);

        GameObject vfx = Instantiate(vfxPrefab, transform.position, quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = shardManager.player.vfx.GetElementColor(usedElement);

        OnExplode?.Invoke();
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
