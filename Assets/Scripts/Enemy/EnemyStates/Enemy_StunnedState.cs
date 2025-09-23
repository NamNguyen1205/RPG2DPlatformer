using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_Vfx vfx;
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        vfx = enemy.GetComponent<Enemy_Vfx>();
    }

    public override void Enter()
    {
        base.Enter();

        vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindon(false);
        stateTimer = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
        
    }
}
