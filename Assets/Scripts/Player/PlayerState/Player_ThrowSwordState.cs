
using UnityEngine;

public class Player_ThrowSwordState : PlayerState
{
    private Camera mainCamera;

    public Player_ThrowSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.attackTrigged = false;
        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();
        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocityY);
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajection(dirToMouse);

        if (controls.PressedAttack())
        {
            anim.SetBool("ThrowSwordPerform", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajection(dirToMouse);
        }

        if (controls.PressedRangeAttack() || player.attackTrigged)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("ThrowSwordPerform", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(controls.mousePosition);

        Vector2 direction = worldMousePosition - playerPos;

        return direction.normalized;
    }
}
