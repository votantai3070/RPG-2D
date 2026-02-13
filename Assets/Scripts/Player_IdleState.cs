public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.controls.moveInput.x == player.faceDir && player.wallDetected)
            return;

        if (player.controls.moveInput.x != 0)
            player.stateMachine.ChangeState(player.moveState);
    }
}
