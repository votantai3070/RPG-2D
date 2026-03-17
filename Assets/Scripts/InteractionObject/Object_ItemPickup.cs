using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private Vector2 dropForce = new(3, 10);
    [Space]
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        SetupVisuals();
    }

    public void SetupItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new(xForce, dropForce.y);
        col.isTrigger = false;
    }

    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = $"Item - {itemData.itemName}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();

        if (inventory == null)
            return;

        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Storage storage = inventory.storage;

        if (itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        if (inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
