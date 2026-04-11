using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;
    public static Action OnPlayerDead;
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX playerVfx { get; private set; }
    public Player_Health playerHealth { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }
    public Player_Stats playerStats { get; private set; }
    public Player_Combat combat { get; private set; }
    public UI ui { get; private set; }
    public Inventory_Player inventory { get; private set; }
    public Player_QuestManager questManager { get; private set; }

    #region State
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
    public Player_ThrowSwordState throwSwordState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }
    #endregion

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

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;

    [Header("Counter Attack Info")]
    public float counterAttackDuration = 1;

    protected override void Awake()
    {
        base.Awake();

        instance = this;

        skillManager = GetComponent<Player_SkillManager>();
        playerVfx = GetComponent<Player_VFX>();
        playerHealth = GetComponent<Player_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        playerStats = GetComponent<Player_Stats>();
        combat = GetComponent<Player_Combat>();
        ui = FindFirstObjectByType<UI>();
        inventory = GetComponent<Inventory_Player>();
        questManager = GetComponent<Player_QuestManager>();

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
        throwSwordState = new(this, stateMachine, "ThrowSword");
        domainExpansionState = new(this, stateMachine, "JumpFall");
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

    public void TryInteract()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);


        foreach (var target in hits)
        {
            if (!target.TryGetComponent<IInteractable>(out var interactable))
                continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        Debug.Log("Closest:  " + closestTarget);

        if (closestTarget == null)
            return;

        closestTarget.GetComponent<IInteractable>().Interact();
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;

    protected override IEnumerator HandleChillCo(float duration, float elementalMultiplier)
    {
        stateHandler.SetElement(ElementType.Ice);

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

        stateHandler.SetElement(ElementType.None);

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
