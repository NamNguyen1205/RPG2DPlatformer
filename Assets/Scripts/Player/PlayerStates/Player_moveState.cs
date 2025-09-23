using UnityEngine;

public class Player_moveState : Player_GroundedState
{
    public Player_moveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocityY);
    }
}
