using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeadVfx;
    [SerializeField] private LayerMask whatIsGround;

    private bool shouldMovePlayer;

    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;
    private Entity_Health playerHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;
    private SkillObject_Health echoHealth;

    public int maxAttacks { get; private set; }

    private void Update()
    {
        if (shouldMovePlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocityY);
            StopHorizontalMovement();
        }
    }
    public virtual void SetupTimeEcho(Skill_TimeEcho timeEchoManager)
    {
        echoManager = timeEchoManager;
        maxAttacks = timeEchoManager.GetMaxAttacks();
        playerStats = timeEchoManager.player.playerStats;
        player = timeEchoManager.player;
        damageScale = timeEchoManager.damageScaleData;
        playerHealth = timeEchoManager.player.entityHealth;
        skillManager = timeEchoManager.skillManager;
        statusHandler = timeEchoManager.player.statusHandler;

        Invoke(nameof(HandleDie), timeEchoManager.GetTimeEchoDuration());
        HandleFlip();

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
        int healAmount = Mathf.RoundToInt(echoHealth.lastDamageTaken * echoManager.GetPercentOfDamageHealed());
        playerHealth.IncreaseHealth(healAmount);

        float amountInSeconds = echoManager.GetCooldownReduceInSecond();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);

        if (echoManager.CanRemoveNegativeEffect())
            statusHandler.RemoveAllNegativeEffects();
    }

    public void HandleDie()
    {
        Instantiate(onDeadVfx, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
            TurnToWisp();
        else
            Destroy(gameObject);
    }

    private void TurnToWisp()
    {
        shouldMovePlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGoHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDulicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }

    private void HandleFlip()
    {
        Transform target = FindClosestTarget();

        if (target == null)
            return;

        if (target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity = new(0, rb.linearVelocityY);
    }
}
