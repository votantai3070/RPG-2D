public class Player_DashState : EntityState
{
    private float orginalGravity;
    private int dashDir;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.durationDash;

        this.dashDir = player.faceDir;
        orginalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        dashDir = controls.moveInput.x != 0 ? (int)controls.moveInput.x : player.faceDir;

        player.SetVelocity(player.dashSpeed * dashDir, 0);
    }

    public override void Update()
    {
        base.Update();

        CancelDashIfNeeded();

        if (stateTimer < 0)
        {
            if (!player.wallDetected)
                if (player.groundDetected)
                    stateMachine.ChangeState(player.idleState);
                else
                    stateMachine.ChangeState(player.fallState);
            else if (player.wallDetected)
                stateMachine.ChangeState(player.wallSlideState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
        rb.gravityScale = orginalGravity;
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
    }
}
