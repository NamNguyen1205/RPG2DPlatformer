using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocityY);
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajectory(dirToMouse);

        //if attack pressed confirm sword throw
        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ComfirmTrajectory(dirToMouse);

            //skill manager create sword
        }

        if (input.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.idleState);

        // if aim btn was released, exit to idle
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePostion = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePostion - playerPosition;

        return direction.normalized;
    }
}
