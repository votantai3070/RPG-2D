using UnityEngine;

public class UI : MonoBehaviour
{
    #region UI Components
    public UI_SkillTooltip skillTooltip { get; private set; }
    public UI_ItemTooltip itemTooltip { get; private set; }
    public UI_StatTooltip statTooltip { get; private set; }
    public UI_SkillTree skillTree { get; private set; }
    public Player player { get; private set; }
    public UI_Inventory inventory { get; private set; }
    public UI_Storage storage { get; private set; }
    public UI_Craft craft { get; private set; }
    public UI_Merchant merchant { get; private set; }
    public UI_Ingame ingame { get; private set; }
    public UI_Options options { get; private set; }
    #endregion
    public GameObject[] uiElements;
    public bool alternativeInput { get; private set; }

    bool skillTreeEnabled;
    bool inventoriesEnabled;

    private void Awake()
    {
        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
        itemTooltip = GetComponentInChildren<UI_ItemTooltip>();
        statTooltip = GetComponentInChildren<UI_StatTooltip>();

        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        inventory = GetComponentInChildren<UI_Inventory>(true);
        storage = GetComponentInChildren<UI_Storage>(true);
        craft = GetComponentInChildren<UI_Craft>(true);
        merchant = GetComponentInChildren<UI_Merchant>(true);
        ingame = GetComponentInChildren<UI_Ingame>(true);
        options = GetComponentInChildren<UI_Options>(true);

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
        skillTree.UnlockDefaultSkills();
    }

    public void OpenOptionsUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(true);
        options.gameObject.SetActive(true);
    }

    public void SwitchToIngameUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        StopPlayerControls(false);
        ingame.gameObject.SetActive(true);

        skillTreeEnabled = false;
        inventoriesEnabled = false;
    }

    public void ToggleSkillTree()
    {
        skillTree.transform.SetAsLastSibling();
        SetTooltipAsLastSibing();

        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControlIfNeeded();
    }

    public void ToggleInventory()
    {
        inventory.transform.SetAsLastSibling();
        SetTooltipAsLastSibing();

        inventoriesEnabled = !inventoriesEnabled;
        inventory.gameObject.SetActive(inventoriesEnabled);
        HideAllTooltips();

        StopPlayerControlIfNeeded();
    }

    public void OpenStorageUI(bool openStorageUI)
    {
        storage.gameObject.SetActive(openStorageUI);
        StopPlayerControls(openStorageUI);

        if (openStorageUI == false)
        {
            craft.gameObject.SetActive(false);
            HideAllTooltips();
        }
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchant.gameObject.SetActive(openMerchantUI);
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
