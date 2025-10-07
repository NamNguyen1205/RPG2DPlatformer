
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public class Entity_Stats : MonoBehaviour
{
    public Stat_SetUpSO defaultStatSetup;

    public stat_ResourceGroup resources;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_MajorGroup major;

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        //elemental cao nhat -> full dame, con lai 50%
        float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * 0.5f;
        float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (element == ElementType.Lightning) ? 0 : lightningDamage * 0.5f;

        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + bonusElementalDamage + weakerElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * 0.5f; //bonus resistance from intelligence: +0.5% per INT

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.LightningRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; //resistance will be cappped at 75%
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = Random.Range(0f, 1f) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage * scaleFactor;

    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.85f; //max mitigation will be capped at 85%

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        //total armor reduction as multiplier (eg: 30/100 = 0.3 - multiplier)
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    //can use in Entity_Health 
    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMapHealth = baseMaxHealth + bonusMaxHealth;

        return finalMapHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f; //each agility point gives you 0.5% of evasion

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f; // Evasion will be capped at 85%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.FireResistance: return defense.fireRes;
            case StatType.IceResistance: return defense.iceRes;
            case StatType.LightningResistance: return defense.LightningRes;

            default:
                Debug.Log($"StatType {type} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update default stat setup assigned")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("no default stat setup assigned");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.fireRes.SetBaseValue(defaultStatSetup.FireResistance);
        defense.iceRes.SetBaseValue(defaultStatSetup.IceResistance);
        defense.LightningRes.SetBaseValue(defaultStatSetup.LightningResistance);
    }



}
