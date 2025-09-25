using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTrigger
{
    private Enemy enemy;
    private Enemy_Vfx enemyVfx;

    override protected void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<Enemy_Vfx>();
    }

    private void EnableCounterWindon()
    {
        enemy.EnableCounterWindon(true);
        enemyVfx.EnableAttackAlert(true);
    }

    private void DisableCounterWindon()
    {
        enemy.EnableCounterWindon(false);
        enemyVfx.EnableAttackAlert(false);
    }
}
