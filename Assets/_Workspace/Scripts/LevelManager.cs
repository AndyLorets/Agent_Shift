using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _startBtn;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText; 
    [SerializeField] private Level[] _levels;
    private int _currentLevel;

    private void Start()
    {
        _currentLevel = ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel;
        SelectLevel();
    }
    private void SelectLevel()
    {
        _image.sprite = _levels[_currentLevel - 1].sprite;
        _descriptionText.text = _levels[_currentLevel - 1].description;
        _titleText.text = $"Mission: {_currentLevel}";
        _startBtn.interactable = _currentLevel <= ServiceLocator.GetService<GameDataController>().PlayerData.openLevel; 
    }
    public void NextLevel()
    {
        if (_currentLevel < _levels.Length)
        {
            _currentLevel++; 
        }
        SelectLevel();
    }
    public void PreviousLevel()
    {
        if (_currentLevel > 1)
        {
            _currentLevel--;
        }
        SelectLevel();
    }
    public void StartLevel()
    {
        ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel = _currentLevel;
        ServiceLocator.GetService<GameDataController>().SaveData();
        ServiceLocator.GetService<SceneLoader>().LoadScene(_currentLevel); 
    }
}
[System.Serializable]
public class Level
{
    [SerializeField] private Sprite _sprite;
    [SerializeField, TextArea(5, 5)] private string _description; 

    public Sprite sprite => _sprite;
    public string description => _description;  
}
