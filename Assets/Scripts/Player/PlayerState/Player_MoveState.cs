
using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        Vector2 input = controls.moveInput;

        if (!controls.PressedAttack())
            player.SetVelocity(input.x * player.moveSpeed, rb.linearVelocityY);

        if (input.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
