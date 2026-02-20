public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.attackTrigged = false;

        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("AttackSpeedMultiplier", attackSpeed);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.attackTrigged && stateMachine.currentState != enemy.counterState)
            stateMachine.ChangeState(enemy.battleState);
    }

}
