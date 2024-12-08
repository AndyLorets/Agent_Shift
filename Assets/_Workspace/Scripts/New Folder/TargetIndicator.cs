using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class TargetIndicator : MonoBehaviour
{
    public RectTransform canvasRect;
    public RectTransform indicator; 
    public Camera mainCamera; 
    public Sprite arrowSprite; 
    public Sprite iconSprite; 
    public Image indicatorImage;
     public Transform target {  get; private set; } 

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        try
        {
            target = GameObject.FindGameObjectWithTag("MiniMapIcon").transform;
        }
        catch { }
        if (target == null)
        {
            gameObject.SetActive(false);    
        }

        _canvasGroup = GetComponent<CanvasGroup>();
        Deactive();

        GameManager.onGameStart += Active;
        GameManager.onGameWin += Deactive;
        GameManager.onGameLose += Deactive;
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= Active;
        GameManager.onGameWin -= Deactive;
        GameManager.onGameLose -= Deactive;
    }
    private void Active()
    {
        if (target == null) return; 

        _canvasGroup.alpha = 0.5f;
        ChangeColorIndicator(); 
    }
    private void Deactive()
    {
        if (target == null) return;
        _canvasGroup.alpha = 0;
    }
    private void ChangeColorIndicator()
    {
        if (_canvasGroup.alpha == 0 || target == null) return; 

        Color color = indicatorImage.color == Color.white ? Color.yellow : Color.white; 
        indicatorImage.DOColor(color, .5f).OnComplete(() => ChangeColorIndicator());
    }
    private void Update()
    {
        if (target == null) return;

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(target.position);

        if (screenPoint.z < 0)
        {
            screenPoint.x = 1f - screenPoint.x;
            screenPoint.y = 1f - screenPoint.y;
            screenPoint.z = Mathf.Abs(screenPoint.z);
        }

        if (screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1)
        {
            // В пределах камеры:
            indicatorImage.sprite = iconSprite;
            Vector2 canvasPosition = WorldToCanvasPosition(screenPoint);
            indicator.anchoredPosition = canvasPosition;
            indicator.localRotation = Quaternion.identity;
        }
        else
        {
            // Вне камеры:
            indicatorImage.sprite = arrowSprite;
            Vector2 canvasPosition = CalculateArrowPosition(screenPoint);
            indicator.anchoredPosition = canvasPosition;
            indicator.localRotation = CalculateArrowRotation(screenPoint);
        }
    }


    private Vector2 WorldToCanvasPosition(Vector3 screenPoint)
    {
        Vector2 viewportPosition = new Vector2(screenPoint.x, screenPoint.y);
        Vector2 canvasSize = canvasRect.sizeDelta;

        return new Vector2(
            viewportPosition.x * canvasSize.x - canvasSize.x / 1.85f,
            viewportPosition.y * canvasSize.y - canvasSize.y / 2.15f
        );
    }

    private Vector2 CalculateArrowPosition(Vector3 screenPoint)
    {
        Vector2 viewportPosition = new Vector2(screenPoint.x, screenPoint.y);
        Vector2 canvasSize = canvasRect.sizeDelta;

        Vector2 rawPosition = new Vector2(
            viewportPosition.x * canvasSize.x,
            viewportPosition.y * canvasSize.y
        );

        float halfWidth = indicator.sizeDelta.x / 2f;
        float halfHeight = indicator.sizeDelta.y / 2f;

        float clampedX = Mathf.Clamp(rawPosition.x, halfWidth, canvasSize.x - halfWidth);
        float clampedY = Mathf.Clamp(rawPosition.y, halfHeight, canvasSize.y - halfHeight);

        return new Vector2(clampedX, clampedY) - canvasSize / 2f;
    }

    private Quaternion CalculateArrowRotation(Vector3 screenPoint)
    {
        Vector2 viewportCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = new Vector2(screenPoint.x, screenPoint.y) - viewportCenter;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }  
}
