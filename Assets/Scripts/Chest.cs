using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Open details")]
    [SerializeField] private Vector2 knockback;
    public bool TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim.SetBool("openChest", true);
        rb.linearVelocity = knockback;

        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
        //drop item
    }
}
