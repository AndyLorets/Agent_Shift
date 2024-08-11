using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static System.Action<bool> onPause;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }
    public void Pause(bool value)
    {
        float t = value ? 0 : 1;
        Time.timeScale = t;
        onPause?.Invoke(value);
    }
}
