using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool couterSomebody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.GetCounterRecoveryDuration();
        couterSomebody = combat.CounterAttackPerformed();
        
        anim.SetBool("counterAttackPerformed", couterSomebody);
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(0, rb.linearVelocityY);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && couterSomebody == false)
            stateMachine.ChangeState(player.idleState);
        
    }
}
