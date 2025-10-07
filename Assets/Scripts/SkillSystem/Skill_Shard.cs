using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Skill_Shard : Skill_Base
{

    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 2;
    [Header("MultiCast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currnetCharges;
    [SerializeField] private bool isRecharging;
    [Header("Teleport shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();
        currnetCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
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
            
        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();
            
        if (Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercent);
            SetSkillOnCoolDown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCoolDown();
        }

    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();

        player.TeleportPlayer(shardPosition);
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
        float detonateTime = GetDetonateTime();

        GameObject shard = Instantiate(shardPrefab, transform.position, quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard()
    {
        bool canMove = Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_MultiCast);


        GameObject shard = Instantiate(shardPrefab, transform.position, quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed);
    }

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCoolDown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }


}
