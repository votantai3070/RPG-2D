public class PlayerState : EntityState
{
    protected Player player;
    protected ControlsManager controls;


    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        controls = player.controls;
    }

    public override void Enter()
    {
        base.Enter();

        player.anim.SetBool(animBoolName, true);
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

        player.anim.SetBool(animBoolName, false);
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
