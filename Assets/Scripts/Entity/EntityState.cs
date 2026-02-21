using UnityEngine;

public class EntityState
{
    protected StateMachine stateMachine;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected string animBoolName;
    protected float stateTimer;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }
}
