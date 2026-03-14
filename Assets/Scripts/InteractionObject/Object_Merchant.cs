using UnityEngine;

public class Object_Merchant : Object_NPC, IInteractable
{
    private Inventory_Merchant merchant;
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<Inventory_Merchant>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z))
            merchant.FillShopList();
    }

    public void Interact()
    {
        Debug.Log("Merchant Interact!");
        ui.merchant.SetupMerchantUI(merchant, inventory);
        ui.merchant.gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = collision.GetComponent<Inventory_Player>();
        merchant.SetupInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllTooltips();
        ui.merchant.gameObject.SetActive(false);
    }
}
