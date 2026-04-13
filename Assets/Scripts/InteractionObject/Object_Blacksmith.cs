using UnityEngine;

public class Object_Blacksmith : Object_NPC, IInteractable
{
    private Inventory_Storage storage;
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("Blacksmith", true);
        storage = GetComponent<Inventory_Storage>();
    }

    public override void Interact()
    {
        base.Interact();

        ui.storageUI.SetupStorageUI(storage);
        ui.craftUI.SetupCraftUI(storage);

        ui.OpenStorageUI(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = collision.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllTooltips();

        ui.OpenStorageUI(false);
    }
}
