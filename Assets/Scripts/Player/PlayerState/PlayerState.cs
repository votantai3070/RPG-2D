public class PlayerState : EntityState
{
    protected Player player;
    protected ControlsManager controls;
    protected Entity_Stats stats;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        controls = player.controls;
        stats = player.entityStat;
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
            stateMachine.ChangeState(player.dashState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanDash()
    {
        if (stateMachine.currentState == player.dashState)
            return false;

        if (player.wallDetected)
            return false;

        return true;
    }

}
