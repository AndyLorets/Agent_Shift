using UnityEngine;

public class CameraUI : MonoBehaviour
{
    private Camera _camera; 
    private void OnEnable()
    {
        GameManager.onGameStart += Active; 
        GameManager.onGameWin += Deactive;
        GameManager.onGameLose += Deactive; 
    }
    private void OnDisable()
    {
        GameManager.onGameStart -= Active;
        GameManager.onGameWin -= Deactive;
        GameManager.onGameLose -= Deactive;
    }
    private void Awake()
    {
        _camera = GetComponent<Camera>();   
    }
    private void Start()
    {
        Deactive(); 
    }
    private void Deactive()
    {
        _camera.enabled = false; 
    }
    private void Active()
    {
        _camera.enabled = true;
    }
}
