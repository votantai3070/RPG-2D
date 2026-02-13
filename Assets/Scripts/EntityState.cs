using UnityEngine;

public class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Rigidbody2D rb;
    protected ControlsManager controls;

    protected float stateTimer;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);

        rb = player.rb;
        controls = player.controls;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        player.anim.SetFloat("yVelocity", rb.linearVelocityY);

        if (controls.PressedDash() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public virtual void Exit()
    {
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
