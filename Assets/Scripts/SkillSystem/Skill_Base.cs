using Unity.VisualScripting;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public DamageScaleData damageScaleData { get; private set; }

    public Player player { get; private set; }
    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed = lastTimeUsed - cooldown;
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;
    }

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;
            
        if (OnCooldown())
        {
            Debug.Log("on cooldown");
            return false;
        }

        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck; 

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;
    public void ResetCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
}
