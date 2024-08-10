using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _progressText;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        ServiceLocator.RegisterService(this);
    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(SceneLoading(sceneIndex));
        _canvasGroup.alpha = 1; 
    }

    private IEnumerator SceneLoading(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            _progressText.text = $"Loading.. {(int)asyncOperation.progress * 100}%";
            yield return null;
        }

        _canvasGroup.alpha = 0; 
    }
}
