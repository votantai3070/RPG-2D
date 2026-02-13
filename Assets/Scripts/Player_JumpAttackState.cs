public class Player_JumpAttackState : EntityState
{
    private bool isAttackAir;

    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.attackTrigged = false;
        isAttackAir = false;

        ApplyJumpAttackVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        AttackIfGrounded();

        if (player.attackTrigged)
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
    }

    private void AttackIfGrounded()
    {
        if (player.groundDetected && !isAttackAir)
        {
            isAttackAir = true;
            player.anim.SetTrigger("JumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocityY);
        }
    }

    private void ApplyJumpAttackVelocity()
    {
        player.SetVelocity(player.jumpAttackVelocity.x * player.faceDir, player.jumpAttackVelocity.y);
    }
}
