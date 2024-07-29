using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item _item;
    private InventoryUI _inventoryUI;

    void Start()
    {
        _inventoryUI = FindObjectOfType<InventoryUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
        {
            _inventoryUI.AddItemToInventory(_item);
            Destroy(gameObject);
        }
    }
}
