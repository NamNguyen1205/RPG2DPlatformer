using System.Security;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity_VFX entityVfx;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats stats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;
    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;//percentageof health you should lose to consider damage as heavy

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack");
            return false;
        }

        Entity_Stats attackStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackStats != null ? attackStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1 - mitigation);

        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        //knockback
        entity?.ReciveKnockback(knockback, duration);
        entityVfx?.PlayOnDamageVfx();

        ReduceHp(finalDamage);
        Debug.Log("damage taken: " + finalDamage);
        
        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;
        healthBar.value = currentHp / stats.GetMaxHealth();
    }
        

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;


        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;
        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() > heavyDamageThreshold;

}
