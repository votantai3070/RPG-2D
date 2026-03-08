using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillTooltip skillTooltip { get; private set; }
    public UI_ItemTooltip itemTooltip { get; private set; }
    public UI_StatTooltip statTooltip { get; private set; }
    public UI_SkillTree skillTree { get; private set; }
    public Player player { get; private set; }
    public UI_Inventory inventory { get; private set; }

    bool skillTreeEnabled;
    bool inventoriesEnabled;

    private void Awake()
    {
        skillTooltip = GetComponentInChildren<UI_SkillTooltip>();
        itemTooltip = GetComponentInChildren<UI_ItemTooltip>();
        statTooltip = GetComponentInChildren<UI_StatTooltip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        player = FindAnyObjectByType<Player>();
        inventory = GetComponentInChildren<UI_Inventory>(true);
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
}
