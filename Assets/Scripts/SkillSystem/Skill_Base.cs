using Unity.VisualScripting;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
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

    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;
    public void ResetCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
}
