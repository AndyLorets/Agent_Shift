using UnityEngine;
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        _fpsText.text = string.Format("{0:0.} FPS", fps);
    }
}
