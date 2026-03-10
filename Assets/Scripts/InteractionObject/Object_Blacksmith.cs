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

    public void Interact()
    {
        Debug.Log("Interact Blacksmith");
        ui.storage.SetupStorage(inventory, storage);
        ui.storage.gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = collision.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }
}
