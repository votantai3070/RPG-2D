public class PlayerState : EntityState
{
    protected Player player;
    protected ControlsManager controls;
    protected Entity_Stats stats;
    protected Player_SkillManager skillManager;
    protected Player_VFX vfx;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        controls = player.controls;
        stats = player.playerStats;
        skillManager = player.skillManager;
        vfx = player.playerVFX;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        player.anim.SetFloat("yVelocity", rb.linearVelocityY);

        if (controls.PressedDash() && CanDash())
        {
            skillManager.skillDash.SetSkillCooldown();
            stateMachine.ChangeState(player.dashState);
        }

        if (controls.PressedUltimateSpell() && skillManager.skillDomain.CanBeUsedSkill())
        {
            if (skillManager.skillDomain.InstantDomain())
            {
                skillManager.skillDomain.CreateDomain();
            }
            else
            {
                stateMachine.ChangeState(player.domainExpansionState);
            }
            skillManager.skillDomain.SetSkillCooldown();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanDash()
    {
        if (!skillManager.skillDash.CanBeUsedSkill())
            return false;

        if (stateMachine.currentState == player.dashState || stateMachine.currentState == player.domainExpansionState)
            return false;

        if (player.wallDetected)
            return false;

        return true;
    }

}
