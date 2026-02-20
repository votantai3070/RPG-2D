public class EnemyState : EntityState
{
    protected Enemy enemy;
    protected Entity_Stats stats;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        stats = enemy.entityStat;
    }
}
