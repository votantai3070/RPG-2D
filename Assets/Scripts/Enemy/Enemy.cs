using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Quest Info")]
    public string questTargetId;

    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_CounterState counterState;

    public Player player { get; private set; }
    public Enemy_Health health { get; private set; }
    public float activeSlowMultiplier { get; private set; } = 1;

    [Header("Enemy Movement Info")]
    public float idleDuration = 2;
    public float moveSpeed = 1.5f;
    [Range(0, 2)]
    public float moveAnimMultilier = 1;
    public float battleSpeed = 3;
    public float retreatDistance = 2;
    public Vector2 retreatDir;

    [Header("Player detected")]
    public float playerDetectedDistance;
    public float attackDistance;
    public LayerMask whatIsPlayer;
    public Transform playerDetectedPoint;

    [Header("Attack Info")]
    public float attackDuration = 2;

    [Header("Counter Info")]
    public float counterDuration = 1;
    public Vector2 knockbackCounterPower = new(5, 5);
    public bool canCounterAttack = false;
    [SerializeField] protected GameObject attackAlert;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Enemy_Health>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        SetupAnimationMultilier();
    }

    private void SetupAnimationMultilier()
    {
        float battleAnimMultilier = battleSpeed / moveSpeed;

        anim.SetFloat("battleAnimMultilier", battleAnimMultilier);
        anim.SetFloat("moveAnimMultilier", moveAnimMultilier);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    protected override IEnumerator HandleChillCo(float duration, float chillMultiplier)
    {
        stateHandler.SetElement(ElementType.Ice);

        activeSlowMultiplier = 1 - chillMultiplier;
        anim.speed *= activeSlowMultiplier;

        yield return new WaitForSeconds(duration);

        stateHandler.SetElement(ElementType.None);
        StopSlowDown();
    }

    public override void StopSlowDown()
    {
        anim.speed = 1;
        activeSlowMultiplier = 1;

        base.StopSlowDown();
    }

    public void TryEnterBattleState(Player player)
    {
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public override void TryEnterDeadState()
    {
        base.TryEnterDeadState();

        stateMachine.ChangeState(deadState);
    }

    private void TryEnterIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public Player GetPlayerReference() => player;

    public RaycastHit2D DetectedPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerDetectedPoint.position, Vector2.right * faceDir, playerDetectedDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    private void OnEnable()
    {
        Player.OnPlayerDead += TryEnterIdleState;
    }

    private void OnDisable()
    {
        Player.OnPlayerDead -= TryEnterIdleState;
    }

    public void EnableAttackAlert(bool enable) => attackAlert.SetActive(enable);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerDetectedPoint.position, new Vector3(playerDetectedPoint.position.x + playerDetectedDistance * faceDir, playerDetectedPoint.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerDetectedPoint.position, new Vector3(playerDetectedPoint.position.x + attackDistance * faceDir, playerDetectedPoint.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerDetectedPoint.position, new Vector3(playerDetectedPoint.position.x + retreatDistance * faceDir, playerDetectedPoint.position.y));
    }

}
