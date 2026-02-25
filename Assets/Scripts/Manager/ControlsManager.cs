using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }

    public PlayerControls inputActions;
    public Player player;
    private UI ui;

    public Vector2 moveInput { get; private set; }

    private void Awake()
    {
        instance = this;
        player = FindAnyObjectByType<Player>();
        inputActions = new PlayerControls();
        ui = FindAnyObjectByType<UI>();
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
        inputActions.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTree();
        inputActions.Player.CastSpell.performed += ctx => player.skillManager.skillShard.CreateShard();
    }

    public bool PressedAttack() => inputActions.Player.Attack.WasPressedThisFrame();

    public bool PressedDash() => inputActions.Player.Dash.WasPressedThisFrame();

    public bool PressedJump() => inputActions.Player.Jump.WasPressedThisFrame();

    public bool PressedCounterAttack() => inputActions.Player.CounterAttack.WasPressedThisFrame();

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
