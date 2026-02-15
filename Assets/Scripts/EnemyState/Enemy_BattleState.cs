using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player == null)
            player = enemy.DetectedPlayer().transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (AttackPlayer())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.moveSpeed * AttackDir(), rb.linearVelocityY);
    }

    private bool AttackPlayer() => DistanceToPlayer() < enemy.attackDistance;

    private float DistanceToPlayer()
    {
        float distance = float.MaxValue;

        if (player != null)
            return Mathf.Abs(player.position.x - enemy.transform.position.x);

        return distance;
    }

    private float AttackDir() => player.position.x > enemy.transform.position.x ? 1 : -1;
}
