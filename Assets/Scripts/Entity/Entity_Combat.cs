using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    public float damage = 10;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {

        foreach (var target in GetDetectedColliders())
        {
            //giup detect ca player, enemy va chest
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            damageable.TakeDamage(damage, transform); // if damageable != null => call TakeDamage
            vfx.CreateOnHitVfx(target.transform);

            // Entity_Health targetHealth = target.GetComponent<Entity_Health>();
            // targetHealth?.TakeDamage(damage, transform);// if targetHealth != null => call TakeDamage

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
