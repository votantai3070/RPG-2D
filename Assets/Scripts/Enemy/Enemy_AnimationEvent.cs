public class Enemy_AnimationEvent : Entity_AnimationEvent
{
    private Enemy enemy;


    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }

    private void EnableCounterAttack()
    {
        enemy.EnableAttackAlert(true);
        enemy.canCounterAttack = true;
    }

    private void DisableCounterAttack()
    {
        enemy.EnableAttackAlert(false);
        enemy.canCounterAttack = false;
    }
}
