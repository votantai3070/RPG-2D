public class Player_DashState : PlayerState
{
    private float orginalGravity;
    private int dashDir;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.skillDash.OnStartEffect();
        vfx.DoImageEchoEffect(player.durationDash);

        stateTimer = player.durationDash;
        orginalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        dashDir = controls.moveInput.x != 0 ? (int)controls.moveInput.x : player.faceDir;

        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * dashDir, 0);

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

        skillManager.skillDash.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = orginalGravity;

        player.health.SetCanTakeDamage(true);
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
