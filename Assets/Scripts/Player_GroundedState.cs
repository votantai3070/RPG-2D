public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.fallState);

        if (controls.inputActions.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);
    }
}
