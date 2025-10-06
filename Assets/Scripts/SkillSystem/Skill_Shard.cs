using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Skill_Shard : Skill_Base
{

    private SkillObject_Shard currentShard;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 2;
    [Header("MultiCast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currnetCharges;
    [SerializeField] private bool isRecharging;

    protected override void Awake()
    {
        base.Awake();
        currnetCharges = maxCharges;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();
        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
            HandleShardMultiCast();
    }

    private void HandleShardMultiCast()
    {
        if (currnetCharges <= 0)
            return;

        CreateShard();
        currentShard.MoveToWardsClosestTarget(shardSpeed);
        currnetCharges--;

        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while (currnetCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currnetCharges++;
        }

        isRecharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveToWardsClosestTarget(shardSpeed);
        SetSkillOnCoolDown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCoolDown();
    }

    public void CreateShard()
    {

        GameObject shard = Instantiate(shardPrefab, transform.position, quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();

        currentShard.SetupShard(detonateTime);
    }
}
