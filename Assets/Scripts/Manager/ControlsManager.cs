using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }

    public PlayerControls inputActions;
    public Player player;
    private UI ui;

    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

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
        //Player
        inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        inputActions.Player.ToggleCharacterUI.performed += ctx => ui.ToggleInventoryUI();

        inputActions.Player.CastSpell.performed += ctx => player.skillManager.shard.TryUseSkill();
        inputActions.Player.CastSpell.performed += ctx => player.skillManager.timeEcho.TryUseSkill();

        inputActions.Player.Interaction.performed += ctx => player.TryInteract();

        inputActions.Player.QuickItemSlot1.performed += ctx => player.inventory.TryUseQuickItemInSlot(1);
        inputActions.Player.QuickItemSlot2.performed += ctx => player.inventory.TryUseQuickItemInSlot(2);


        //UI
        inputActions.UI.SkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        inputActions.UI.InventoryUI.performed += ctx => ui.ToggleInventoryUI();

        inputActions.UI.AlternativeInput.performed += ctx => ui.SetAlternativeInput(true);
        inputActions.UI.AlternativeInput.canceled += ctx => ui.SetAlternativeInput(false);

        inputActions.UI.OptionUI.performed += ctx =>
        {
            foreach (var element in ui.uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    ui.SwitchToIngameUI();
                    return;
                }

            }

            Time.timeScale = 0;
            ui.OpenOptionsUI();
        };

        inputActions.UI.DialogueUI.performed += ctx =>
        {
            if (ui.dialogueUI.gameObject.activeInHierarchy)
                ui.dialogueUI.DialogueInteraction();
        };
    }

    public bool PressedAttack() => inputActions.Player.Attack.WasPressedThisFrame();

    public bool PressedDash() => inputActions.Player.Dash.WasPressedThisFrame();

    public bool PressedJump() => inputActions.Player.Jump.WasPressedThisFrame();

    public bool PressedCounterAttack() => inputActions.Player.CounterAttack.WasPressedThisFrame();

    public bool PressedRangeAttack() => inputActions.Player.RangeAttack.WasPressedThisFrame();

    public bool PressedUltimateSpell() => inputActions.Player.UltimateSpell.WasPressedThisFrame();


    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
