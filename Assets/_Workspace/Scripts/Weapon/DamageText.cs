using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private const float DISPLAY_TIME = 1.2f;
    private const float MOVE_SPEED = 2.0f;

    private TextMeshProUGUI _damageText;
    private DamageTextPool _damageTextPool;
    private Vector3 _startLocalPos; 

    public void Init(DamageTextPool damageTextPool)
    {
        _damageTextPool = damageTextPool;
        _startLocalPos = transform.localPosition;
        _damageText = GetComponent<TextMeshProUGUI>(); 
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), DISPLAY_TIME);
    }
    private void OnDisable()
    {
        transform.localPosition = _startLocalPos; 
    }

    private void Update()
    {
        transform.position += Vector3.up * MOVE_SPEED * Time.deltaTime;
    }

    public void SetDamageAmount(float damage)
    {
        _damageText.text = $"-{damage}";
    }

    private void ReturnToPool()
    {
        _damageTextPool.ReturnDamageText(this);
    }
}
