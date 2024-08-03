using UnityEngine;

public class ItemPickup : MonoBehaviour, ITaskable
{
    [SerializeField] private Item _item;
    private InventoryUI _inventoryUI;

    public string taskName { get; set; }

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

            if (taskName != "")
                TaskManager.Instance.CompleteTask(taskName);
        }
    }
}
