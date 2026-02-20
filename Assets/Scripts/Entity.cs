using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity_VFX vfx { get; private set; }
    public Entity_ElementalStateHandler stateHandler { get; private set; }
    public Entity_Health entityHealth { get; private set; }
    public Entity_Stats entityStat { get; private set; }

    public static Action OnFlipped;
    public StateMachine stateMachine { get; private set; }
    public ControlsManager controls { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    private bool isFacingRight = true;
    public int faceDir { get; private set; } = 1;
    [HideInInspector] public bool attackTrigged;


    [Header("Collision Check")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    [SerializeField] private Transform groundCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    [Header("Knockback")]
    [SerializeField] private Vector2 knockBackPower;
    [SerializeField] private Vector2 heavyKnockBackPower;
    [SerializeField] private float knockBackDuration = .1f;
    private bool isKnockBack;
    private Coroutine knockBackCo;
    [SerializeField] private float heavyKnockBackThreshold = .3f;

    [Header("Elemental Info")]
    private Coroutine elementalEffectCo;


    protected virtual void Awake()
    {
        entityHealth = GetComponent<Entity_Health>();
        vfx = GetComponent<Entity_VFX>();
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        controls = ControlsManager.instance;
        stateHandler = GetComponent<Entity_ElementalStateHandler>();
        entityStat = GetComponent<Entity_Stats>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.currentState.Update();
    }

    public virtual void TryEnterDeadState()
    {

    }


    #region Elemental Effects
    public void ElementalVfx(float duration, ElementType element)
    {
        vfx.ElementVfx(duration, element); // Apply elemental VFX based on the element type and duration
    }

    public virtual void EnterChillEffect(float duration, float elementalMultiplier)
    {
        if (elementalEffectCo != null)
            StopCoroutine(elementalEffectCo);
        elementalEffectCo = StartCoroutine(HandleChillCo(duration, elementalMultiplier));
    }

    protected virtual IEnumerator HandleChillCo(float duration, float elementalMultiplier)
    {
        yield return null;
    }
    #endregion

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround)
                 && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround);

    }

    #region Movement and Flip
    public void SetVelocity(float x, float y)
    {
        if (isKnockBack)
            return;

        rb.linearVelocity = new(x, y);
        HandleFlip(x);
    }

    protected void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !isFacingRight)
            Flip();
        else if (xVelocity < 0 && isFacingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        faceDir *= -1;

        OnFlipped?.Invoke();
    }
    #endregion

    #region Knockback
    public void KnockBack(Transform damagedDealer, float averangeDamage)
    {
        if (knockBackCo != null)
            StopCoroutine(knockBackCo);

        Vector2 knockbackDir = KnockBackDir(damagedDealer, averangeDamage);

        knockBackCo = StartCoroutine(KnockBackCo(knockbackDir, knockBackDuration));
    }

    private IEnumerator KnockBackCo(Vector2 knockbackDir, float duration)
    {
        isKnockBack = true;
        rb.linearVelocity = knockbackDir;
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        isKnockBack = false;
    }

    private Vector2 KnockBackDir(Transform damagedDealer, float averageDamage)
    {
        float direction = damagedDealer.position.x > transform.position.x ? -1 : 1;

        Vector2 knockback = averageDamage > heavyKnockBackThreshold ? heavyKnockBackPower : knockBackPower;

        knockback.x *= direction;

        return knockback;
    }
    #endregion

    public void CallAnimationEventAttackOver() => attackTrigged = true;

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * faceDir, 0));
        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * faceDir, 0));
    }
}
