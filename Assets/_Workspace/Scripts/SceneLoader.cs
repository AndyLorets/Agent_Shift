using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Transform _loadingIcon;
    [SerializeField] private TMP_InputField levelInputField;

    private CanvasGroup _canvasGroup;
    private GameDataController _gameData; 

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _gameData = ServiceLocator.GetService<GameDataController>();

        if (_gameData == null)
        {
            Debug.LogError("GameDataController service not found!");
            return;
        }

        bool hasTutorial = PlayerPrefs.HasKey("Tutorial");
        int sceneIndex = hasTutorial ? ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel : SceneManager.sceneCountInBuildSettings - 2;

        LoadScene(sceneIndex);
    }
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(SceneLoading(sceneIndex));
        _canvasGroup.alpha = 1; 
    }

    private IEnumerator SceneLoading(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;  

        float minimumLoadTime = 3f; 
        float elapsedTime = 0f;

        while (!asyncOperation.isDone)
        {
            _loadingIcon.Rotate(-Vector3.forward * 70 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= minimumLoadTime && asyncOperation.progress >= 0.9f)
            {
                ServiceLocator.ClearAllServices();
                ServiceLocator.RegisterService(this);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        _canvasGroup.alpha = 0;
    }

}
