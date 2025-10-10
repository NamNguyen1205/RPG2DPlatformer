using NUnit.Framework.Constraints;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGroud;
    private Skill_TimeEcho echoManager;

    public int maxAttacks { get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();

        FlipToTarget();
        anim.SetBool("canAttack", maxAttacks > 0);
        Invoke(nameof(HandleDeath), echoManager.GetTimeEchoDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        StopHorizontalMovement();
    }

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGroud);

        if (hit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
    }
}
