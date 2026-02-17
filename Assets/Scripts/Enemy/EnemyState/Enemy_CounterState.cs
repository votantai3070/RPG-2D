public class Enemy_CounterState : EnemyState
{
    public Enemy_CounterState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new(enemy.knockbackCounterPower.x * -enemy.faceDir, enemy.knockbackCounterPower.y);

        stateTimer = enemy.counterDuration;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
