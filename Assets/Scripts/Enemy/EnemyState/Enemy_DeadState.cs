using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        anim.enabled = false;
        rb.linearVelocity = new(rb.linearVelocityX, 20);
        rb.gravityScale = 5;
        enemy.GetComponent<Collider2D>().enabled = false;
    }
}
