public class Player_GroundedState : PlayerState
{
    bool canCounterAttack = false;
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCounterAttack = false;
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

        if (controls.PressedCounterAttack() && !canCounterAttack)
        {
            canCounterAttack = true;
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
