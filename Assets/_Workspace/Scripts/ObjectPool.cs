using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private Queue<GameObject> _poolQueue = new Queue<GameObject>();

    private const int INITIAL_SIZE = 10;

    private void Start()
    {
        for (int i = 0; i < INITIAL_SIZE; i++)
        {
            GameObject obj = Instantiate(_prefab);
            obj.SetActive(false);
            obj.transform.parent = transform;
            _poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (_poolQueue.Count > 0)
        {
            GameObject obj = _poolQueue.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(_prefab, position, rotation);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}
