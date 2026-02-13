
using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Vector2 input = controls.moveInput;

        if (!controls.PressedAttack())
            player.SetVelocity(input.x * player.moveSpeed, rb.linearVelocityY);

        if (input.x == 0)
            stateMachine.ChangeState(player.idleState);
    }
}
