using UnityEngine;

public class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected float stateTimer;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

    }

    public virtual void Exit()
    {

    }
}
