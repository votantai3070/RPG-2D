using UnityEngine;

public class Player_AttackState : EntityState
{
    private float attackTimer;
    public Player_AttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GenerateAttackVelocity();
        attackTrigged = false;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
            if (attackTrigged)
                stateMachine.ChangeState(player.idleState);
    }

    private void GenerateAttackVelocity()
    {
        attackTimer = player.durationAttack;
        player.SetVelocity(player.attackVelocity * player.attackVelocitySpeed * player.faceDir, 0);
    }
}
