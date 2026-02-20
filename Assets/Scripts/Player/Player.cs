using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static Action OnPlayerDead;

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }


    [Header("Player Movement Info")]
    public Vector2 jumpForceDir;
    public float moveSpeed = 3;
    public float jumpForce = 8;
    public float dashSpeed = 15;
    public float durationDash = 2;
    [Range(0f, 1f)]
    public float moveAirMultiplier = 0.5f;
    [Range(0f, 1f)]
    public float wallSlideMultiplier = .4f;

    [Header("Player Attack Info")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    private Coroutine basicAttackCo;
    public float durationAttack = 1;
    public int cooldownAttack = 2;

    [Header("Counter Attack Info")]
    public float counterAttackDuration = 1;

    protected override void Awake()
    {
        base.Awake();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        jumpState = new(this, stateMachine, "JumpFall");
        fallState = new(this, stateMachine, "JumpFall");
        wallSlideState = new(this, stateMachine, "WallSlide");
        wallJumpState = new(this, stateMachine, "JumpFall");
        dashState = new(this, stateMachine, "Dash");
        basicAttackState = new(this, stateMachine, "BasicAttack");
        jumpAttackState = new(this, stateMachine, "JumpAttack");
        deadState = new(this, stateMachine, "Dead");
        counterAttackState = new(this, stateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator HandleChillCo(float duration, float elementalMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalDashSpeed = dashSpeed;
        float originalAnimSpeed = anim.speed;
        Vector2[] originalAttackVelocity = attackVelocity;
        Vector2 originalJumpAttackVelocity = jumpAttackVelocity;
        Vector2 originalJumpForceDir = jumpForceDir;

        moveSpeed *= elementalMultiplier;
        jumpForce *= elementalMultiplier;
        dashSpeed *= elementalMultiplier;
        anim.speed *= elementalMultiplier;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= elementalMultiplier;
        }
        jumpAttackVelocity *= elementalMultiplier;
        jumpForceDir *= elementalMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        dashSpeed = originalDashSpeed;
        anim.speed = originalAnimSpeed;
        attackVelocity = originalAttackVelocity;
        jumpAttackVelocity = originalJumpAttackVelocity;
        jumpForceDir = originalJumpForceDir;
    }

    public override void TryEnterDeadState()
    {
        base.TryEnterDeadState();
        OnPlayerDead?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void BasicAttackDelay()
    {
        if (basicAttackCo != null)
            StopCoroutine(basicAttackCo);

        basicAttackCo = StartCoroutine(BasicAttackDelayCo());
    }

    private IEnumerator BasicAttackDelayCo()
    {
        yield return new WaitForEndOfFrame();

        stateMachine.ChangeState(basicAttackState);
    }
}
