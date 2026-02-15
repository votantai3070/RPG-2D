public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.jumpForceDir.x * -player.faceDir, player.jumpForceDir.y);
        player.Flip();
    }

    public override void Update()
    {
        base.Update();

        if (!player.wallDetected && rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.fallState);

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
