using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup; 
    private void OnEnable()
    {
        BriefingManager.onEndBriefing += OnStartGame;

        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false; 
    }
    private void OnDisable()
    {
        BriefingManager.onEndBriefing -= OnStartGame;
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        TaskManager.Instance.onTasksComplate += Restart; 
    }
    private void OnStartGame()
    {
        StartCoroutine(StartGame()); 
    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);

        Camera.main.transform.eulerAngles = new Vector3(45, 0, 0);
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }
    private void OnDestroy()
    {
        TaskManager.Instance.onTasksComplate -= Restart;
    }
    public void Restart()
    {
        CharacterDialogue.speaking = false;
        SceneManager.LoadScene(0);
    }
}
