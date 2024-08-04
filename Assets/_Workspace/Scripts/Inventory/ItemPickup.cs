using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Item _item;
    private InventoryUI _inventoryUI;

    public void Initialize(Item item)
    {
        _item = item;
        _inventoryUI = FindObjectOfType<InventoryUI>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
        {
            _inventoryUI.AddItemToInventory(_item);
            ServiceLocator.GetService<AudioManager>().PlayItem(); 
            Destroy(gameObject);
        }
    }
}
