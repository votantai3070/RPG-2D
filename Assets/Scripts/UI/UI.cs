using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance { get; private set; }

    #region UI Components
    public UI_SkillTooltip skillTooltip { get; private set; }
    public UI_ItemTooltip itemTooltip { get; private set; }
    public UI_StatTooltip statTooltip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public Player player { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_Ingame ingameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScreen deathScreenUI { get; private set; }
    public UI_FadeScreen fadeUI { get; private set; }
    #endregion

    public GameObject[] uiElements;
    public bool alternativeInput { get; private set; }

    bool skillTreeEnabled;
    bool inventoriesEnabled;

    private void Awake()
    {
        instance = this;

        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
        itemTooltip = GetComponentInChildren<UI_ItemTooltip>();
        statTooltip = GetComponentInChildren<UI_StatTooltip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);
        fadeUI = GetComponentInChildren<UI_FadeScreen>(true);

        player = FindAnyObjectByType<Player>();
    }

    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            ControlsManager.instance.inputActions.Player.Disable();
        else
            ControlsManager.instance.inputActions.Player.Enable();
    }

    private void StopPlayerControlIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }

    private void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        ControlsManager.instance.inputActions.Disable();
    }

    public void OpenOptionsUI()
    {
        HideAllTooltips();
        StopPlayerControls(true);
        SwitchTo(optionsUI.gameObject);
    }

    public void SwitchToIngameUI()
    {
        StopPlayerControls(false);

        SwitchTo(ingameUI.gameObject);
        skillTreeEnabled = false;
        inventoriesEnabled = false;
    }

    private void SwitchTo(GameObject objectSwitching)
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        objectSwitching.SetActive(true);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetTooltipAsLastSibing();
        fadeUI.transform.SetAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControlIfNeeded();
    }

    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipAsLastSibing();
        fadeUI.transform.SetAsLastSibling();

        inventoriesEnabled = !inventoriesEnabled;
        inventoryUI.gameObject.SetActive(inventoriesEnabled);
        HideAllTooltips();

        StopPlayerControlIfNeeded();
    }

    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControls(openStorageUI);

        if (openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);
            HideAllTooltips();
        }
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(openMerchantUI);

        if (openMerchantUI == false)
            HideAllTooltips();
    }

    public void HideAllTooltips()
    {
        itemTooltip.ShowTooltip(false, null);
        statTooltip.ShowTooltip(false, null);
        skillTooltip.ShowTooltip(false);
    }

    private void SetTooltipAsLastSibing()
    {
        itemTooltip.transform.SetAsLastSibling();
        skillTooltip.transform.SetAsLastSibling();
        statTooltip.transform.SetAsLastSibling();
    }

    public void SetAlternativeInput(bool enabled) => alternativeInput = enabled;
}
