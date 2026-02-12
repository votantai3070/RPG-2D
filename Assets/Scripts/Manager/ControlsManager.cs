using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }

    public PlayerControls inputActions;
    private Player player;

    public Vector2 moveInput { get; private set; }

    private void Awake()
    {
        instance = this;

        inputActions = new PlayerControls();
        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        AssignInputEvents();
    }

    public void AssignInputEvents()
    {
        // Movement
        inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => player.jumpPressed = true;
        inputActions.Player.Jump.canceled += ctx => player.jumpPressed = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
