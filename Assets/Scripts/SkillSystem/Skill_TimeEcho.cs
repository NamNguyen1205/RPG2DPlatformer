using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Skill_TimeEcho : Skill_Base
{
    
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;
    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttack = 3;
    [SerializeField] private float duplicateChance = 0.3f;
    [Header("Weal wisp upgrades")]
    [SerializeField] private float damagePercentHealed = 0.3f;
    [SerializeField] private float cooldownReduceInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp)
            return 0;

        return cooldownReduceInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TImeEcho_CleanseWisp;
    }

    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp
            || upgradeType == SkillUpgradeType.TImeEcho_CleanseWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToDuplicate)
            return 0;

        return duplicateChance;
    }
    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack || upgradeType == SkillUpgradeType.TimeEcho_ChanceToDuplicate)
            return 1;

        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttack;

        return 0;
    }

    public float GetTimeEchoDuration()
    {
        return timeEchoDuration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreateTimeEcho();
    }

    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);

    }
}
