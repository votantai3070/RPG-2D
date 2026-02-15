using UnityEngine;

public class Entity : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }
    public ControlsManager controls { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    private bool isFacingRight = true;
    public int faceDir { get; private set; } = 1;
    [HideInInspector] public bool attackTrigged;


    [Header("Collision Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        controls = ControlsManager.instance;
    }

    protected virtual void Update()
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
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround)
             && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround);
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

    public void CallAnimationEventAttackOver() => attackTrigged = true;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * faceDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * faceDir, 0));
    }
}
