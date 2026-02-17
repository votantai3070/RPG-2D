using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "Idle");
        moveState = new Enemy_MoveState(this, stateMachine, "Move");
        attackState = new Enemy_AttackState(this, stateMachine, "Attack");
        battleState = new Enemy_BattleState(this, stateMachine, "Battle");
        deadState = new Enemy_DeadState(this, stateMachine, "empty");
        counterState = new Enemy_CounterState(this, stateMachine, "KnockbackCounter");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Q))
            HandleCounter();
    }


    public void HandleCounter()
    {
        if (!canCounterAttack)
            return;

        EnableAttackAlert(false);
        canCounterAttack = false;
        stateMachine.ChangeState(counterState);
    }
}
