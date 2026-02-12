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
    public bool jumpPressed;
    public float jumpForce = 8;

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
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new(x, y);
        HandleFlip();
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
}
