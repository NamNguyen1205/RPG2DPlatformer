using UnityEngine;

public class Object_ItemPIckUp : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSO itemData;

    private Inventory_Item itemToAdd;
    private Inventory_Base Inventory;

    private void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    void OnValidate()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory = collision.GetComponent<Inventory_Base>();

        if (Inventory == null)
            return;

        bool canAddItem = Inventory.CanAddItem() || Inventory.CanAddToStack(itemToAdd);

        if(canAddItem)
        {
            Inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
