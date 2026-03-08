using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] statSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        statSlots = GetComponentsInChildren<UI_StatSlot>(true);
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    private void Start()
    {
        inventory.OnInventoryChange += UpdateStatUI;
        UpdateStatUI();
    }

    private void UpdateStatUI()
    {
        foreach (var stat in statSlots)
        {
            stat.UpdateStatValue();
        }
    }
}
