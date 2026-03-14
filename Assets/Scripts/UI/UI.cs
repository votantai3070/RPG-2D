using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTooltip skillTooltip { get; private set; }
    public UI_ItemTooltip itemTooltip { get; private set; }
    public UI_StatTooltip statTooltip { get; private set; }
    public UI_SkillTree skillTree { get; private set; }
    public Player player { get; private set; }
    public UI_Inventory inventory { get; private set; }
    public UI_Storage storage { get; private set; }
    public UI_Craft craft { get; private set; }
    public UI_Merchant merchant { get; private set; }

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

        player = FindAnyObjectByType<Player>();
    }

    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillTooltip.ShowTooltip(false);
    }

    public void ToggleInventory()
    {
        inventoriesEnabled = !inventoriesEnabled;
        inventory.gameObject.SetActive(inventoriesEnabled);
        itemTooltip.ShowTooltip(false, null);
        statTooltip.ShowTooltip(false, null);
    }

    public void SwitchOffAllTooltips()
    {
        itemTooltip.ShowTooltip(false, null);
        statTooltip.ShowTooltip(false, null);
        skillTooltip.ShowTooltip(false);
    }
}
