public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.wallDetected)
            player.SetVelocity(controls.moveInput.x, rb.linearVelocityY * player.wallSlideMultiplier);

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip();
        }

        if (!player.wallDetected && rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.fallState);

        if (controls.inputActions.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);
    }
}
