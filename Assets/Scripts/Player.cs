using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ControlsManager controls { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public StateMachine stateMachine { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }

    [Header("Player Movement Info")]
    public float moveSpeed = 3;
    public float jumpForce = 8;
    public float dashSpeed = 15;
    public float durationDash = 2;

    public Vector2 jumpForceDir;
    [Range(0f, 1f)]
    public float moveAirMultiplier = 0.5f;
    [Range(0f, 1f)]
    public float wallSlideMultiplier = .4f;
    public int faceDir { get; private set; } = 1;
    private bool isFacingRight = true;

    [Header("Player Attack Info")]
    public float durationAttack = 1;
    public int cooldownAttack = 2;
    public Vector2[] attackVelocity;
    private Coroutine basicAttackCo;

    [Header("Collision Check")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private void Awake()
    {
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        jumpState = new(this, stateMachine, "JumpFall");
        fallState = new(this, stateMachine, "JumpFall");
        wallSlideState = new(this, stateMachine, "WallSlide");
        wallJumpState = new(this, stateMachine, "JumpFall");
        dashState = new(this, stateMachine, "Dash");
        basicAttackState = new(this, stateMachine, "BasicAttack");
    }

    private void Start()
    {
        controls = ControlsManager.instance;

        stateMachine.InitializeState(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.currentState.Update();
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

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new(x, y);
        HandleFlip();
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround);
    }

    private void HandleFlip()
    {
        if (controls.moveInput.x > 0 && !isFacingRight)
            Flip();
        else if (controls.moveInput.x < 0 && isFacingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        faceDir *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * faceDir, 0));
    }
}
