using UnityEngine;
using UnityEngine.UI;

public class UnitInventory : MonoBehaviour, ITaskable
{
    [SerializeField] private Item _item;
    [SerializeField] private Image _icon;

    private ItemPickup _itemPickup; 
    public string taskName { get; set; }

    private void Start()
    {
        _icon.sprite = _item.icon;
        _itemPickup = Instantiate(_item.itemPickup);
        _itemPickup.Initialize(_item);
        _itemPickup.gameObject.SetActive(false);
    }
    public void SpawnItem()
    {
        _itemPickup.gameObject.SetActive(true); 
        _itemPickup.transform.position = transform.position;
        gameObject.SetActive(false);

        if (taskName != "")
            ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
    }
}
