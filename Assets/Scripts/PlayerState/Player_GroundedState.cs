public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (rb.linearVelocityY < 0 && !player.groundDetected)
            stateMachine.ChangeState(player.fallState);

        if (controls.PressedJump())
            stateMachine.ChangeState(player.jumpState);

        if (controls.PressedAttack())
            stateMachine.ChangeState(player.basicAttackState);
    }
}
