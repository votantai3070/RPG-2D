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

    [Header("Player Movement Info")]
    public float moveSpeed = 3;
    private bool isFacingRight = true;
    public float jumpForce = 8;
    [Range(0f, 1f)]
    public float moveAirMultiplier = 0.5f;

    [Header("Collision Check")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    //[SerializeField] private float 
    public bool isGrounded { get; private set; }

    private void Awake()
    {
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        jumpState = new(this, stateMachine, "JumpFall");
        fallState = new(this, stateMachine, "JumpFall");
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

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new(x, y);
        HandleFlip();
    }

    private void HandleCollisionDetection()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleFlip()
    {
        if (controls.moveInput.x > 0 && !isFacingRight)
            Flip();
        else if (controls.moveInput.x < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }
}
