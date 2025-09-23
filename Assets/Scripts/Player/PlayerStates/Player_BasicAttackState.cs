using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private bool comboAttackQueued;
    private int attackDir;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private const int FirstComboIndex = 1;


    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

        if (comboLimit != player.attackVelocity.Length)
        {
            //tranh loi out of range
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetConboIndexIfNeeded();
        //define attack direction according to input
        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        //detect and damage enemy

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleExitState();
    }

    private void HandleExitState()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    private void ResetConboIndexIfNeeded()
    {   //quá combo reset về 1 or thời gian để combo quá reset
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
}
