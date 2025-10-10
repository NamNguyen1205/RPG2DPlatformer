using System.Net.Sockets;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();
        
        // if (player == null)
        //     player = enemy.GetPlayerReference();
        player ??= enemy.GetPlayerReference(); //the different written way of above demand

        if (ShouldRetreat())
            {
                rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirecionToPlayer(), enemy.retreatVelocity.y);
                enemy.HandleFlip(DirecionToPlayer());
            }

    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithInAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirecionToPlayer(), rb.linearVelocityY);
    }
    
    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if(newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }


    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    private bool WithInAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance; 

    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;


        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirecionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
