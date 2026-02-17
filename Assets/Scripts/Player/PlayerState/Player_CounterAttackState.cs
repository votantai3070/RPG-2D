public class Player_CounterAttackState : PlayerState
{
    Player_Combat combat;
    bool canCounterAttack;

    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        player.attackTrigged = false;

        stateTimer = player.counterAttackDuration;
        canCounterAttack = combat.CounterAttackPerform();

        anim.SetBool("CounterAttakcPerform", canCounterAttack);
    }

    public override void Update()
    {
        base.Update();


        if (player.attackTrigged)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && !canCounterAttack)
            stateMachine.ChangeState(player.idleState);
    }
}
