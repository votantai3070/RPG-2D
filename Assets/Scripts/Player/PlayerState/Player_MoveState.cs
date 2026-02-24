public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();


        if (!controls.PressedAttack() || stateMachine.currentState != player.jumpState)
            player.SetVelocity(controls.moveInput.x * player.moveSpeed, rb.linearVelocityY);

        if (controls.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
