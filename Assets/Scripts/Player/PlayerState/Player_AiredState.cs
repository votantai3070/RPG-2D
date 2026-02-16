public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);

        if (controls.moveInput.x != 0 && !controls.PressedDash())
            player.SetVelocity
                (controls.moveInput.x * player.moveSpeed * player.moveAirMultiplier,
                rb.linearVelocityY);

        if (controls.PressedAttack())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}
