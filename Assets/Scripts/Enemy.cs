using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    [Header("Enemy Movement Info")]
    public float idleDuration = 2;
    public float moveSpeed = 1.5f;
    [Range(0, 2)]
    public float moveAnimMultilier = 1;
    public float battleSpeed = 3;


    [Header("Player detected")]
    public float playerDetectedDistance;
    public float attackDistance;
    public LayerMask whatIsPlayer;
    public Transform playerDetectedPoint;

    [Header("Attack Info")]
    public float attackDuration = 2;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        float battleAnimMultilier = battleSpeed / moveSpeed;

        anim.SetFloat("battleAnimMultilier", battleAnimMultilier);
        anim.SetFloat("moveAnimMultilier", moveAnimMultilier);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    public RaycastHit2D DetectedPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerDetectedPoint.position, Vector2.right * faceDir, playerDetectedDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerDetectedPoint.position, new Vector3(playerDetectedPoint.position.x + playerDetectedDistance * faceDir, playerDetectedPoint.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerDetectedPoint.position, new Vector3(playerDetectedPoint.position.x + attackDistance * faceDir, playerDetectedPoint.position.y));
    }
}
