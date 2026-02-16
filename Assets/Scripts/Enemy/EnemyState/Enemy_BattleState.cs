using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastAttackTime;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player = GetPlayer();

        if (WithinRetreatRange())
            rb.linearVelocity = new(enemy.retreatDir.x * -enemy.faceDir, enemy.retreatDir.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (StopAttackPlayer())
            stateMachine.ChangeState(enemy.idleState);

        if (enemy.DetectedPlayer())
            lastAttackTime = Time.time;

        if (AttackPlayer() && enemy.DetectedPlayer())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleSpeed * AttackDir(), rb.linearVelocityY);

    }

    private Transform GetPlayer()
    {
        if (player == null && enemy.GetPlayerReference())
            return enemy.GetPlayerReference().transform;

        return enemy.DetectedPlayer().transform;
    }

    private bool StopAttackPlayer() => Time.time > lastAttackTime + enemy.attackDuration;

    private bool AttackPlayer() => DistanceToPlayer() < enemy.attackDistance;

    private bool WithinRetreatRange() => DistanceToPlayer() < enemy.retreatDistance;

    private float DistanceToPlayer()
    {
        float distance = float.MaxValue;

        if (player != null)
            return Mathf.Abs(player.position.x - enemy.transform.position.x);

        return distance;
    }

    private float AttackDir()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
