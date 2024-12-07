using UnityEngine;
using UnityEngine.UI;

public class UnitInventory : MonoBehaviour, ITaskable
{
    [SerializeField] private Item _item;
    [SerializeField] private Image _icon;

    private ItemPickup _itemPickup; 
    public string taskName { get; set; }
    public bool activeTask { get; set; }

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

        if (taskName != "" && activeTask)
            ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
    }

    public void ActiveTask()
    {
        activeTask = true; 
    }

    public void DeactiveTask()
    {
        activeTask = false; 
    }
}
