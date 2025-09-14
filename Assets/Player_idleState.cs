using UnityEngine;

public class Player_idleState : Player_GroundedState
{
    public Player_idleState(Player player ,StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);


    }
}
