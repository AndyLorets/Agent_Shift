using UnityEngine;

public class Door : MonoBehaviour
{
    private Animation _animation;
    private Collider _collider; 
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        InteractionManager.Interact(Open, true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        InteractionManager.Interact(Open, false);
    }

    private void Open()
    {
        bool canOpen = false; 
        foreach (var t in Inventory.items)
        {
            if(t.itemName == "Door Key")
            {
                canOpen = true;
                _animation.Play();
                _collider.enabled = false;
                InteractionManager.Interact(Open, false);
            }
        }
        if (!canOpen)
            Debug.Log("Нужен Ключ!");
    }
}
