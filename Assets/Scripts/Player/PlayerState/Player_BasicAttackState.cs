using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private const int FIRST_BASIC_ATTACK_INDEX = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;

    private bool attackQueued;

    private float lastAttackTime;
    private float attackTimer;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit > player.attackVelocity.Length)
            comboLimit = player.attackVelocity.Length;
    }

    public override void Enter()
    {
        base.Enter();

        float attackSpeed = stats.offense.attackSpeed.GetValue();

        anim.SetFloat("AttackSpeedMultiplier", attackSpeed);

        attackQueued = false;
        player.attackTrigged = false;

        ResetBasicAttack();

        player.anim.SetInteger("BasicAttackIndex", comboIndex);

        GenerateAttackVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (controls.PressedAttack())
            attackQueued = true;

        if (player.attackTrigged)
            if (attackQueued)
            {
                player.anim.SetBool("BasicAttack", false); // Remove anim frame before
                player.BasicAttackDelay(); // wait current frame end 
            }
            else
                stateMachine.ChangeState(player.idleState);
    }

    private void ResetBasicAttack()
    {
        if (Time.time > lastAttackTime + player.cooldownAttack)
            comboIndex = FIRST_BASIC_ATTACK_INDEX;

        if (comboIndex > comboLimit)
            comboIndex = FIRST_BASIC_ATTACK_INDEX;
    }

    private void HandleAttackVelocity()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
            player.SetVelocity(0, rb.linearVelocityY);
    }

    private void GenerateAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        float attackDir = controls.moveInput.x != 0 ? controls.moveInput.x : player.faceDir;

        attackTimer = player.durationAttack;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
}
