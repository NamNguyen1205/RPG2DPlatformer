using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;
    public float damage = 10;

    public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;
    

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {

        foreach (var target in GetDetectedColliders())
        {
            //giup detect ca player, enemy va chest
            IDamageable damegable = target.GetComponent<IDamageable>();

            if (damegable == null)
                continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physDamage = attackData.physicalDamage;
            //float elementalDamage = stats.GetElementalDamage(out ElementType element, 0.6f);
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            bool targetGotHit = damegable.TakeDamage(physDamage, elementalDamage,element, transform); // if damageable != null => call TakeDamage

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
                vfx.CreateOnHitVfx(target.transform, attackData.isCrit, element);
        }
    }


    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
