using UnityEngine;

public class ShootCondition : TutorialCondition
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Joystick _aimJoysick;
    [SerializeField] private Joystick _moveJoysick;
    private Canvas _canvas;
    private bool _ready;

    public override void EnableCondition()
    {
        base.EnableCondition();

        _aimJoysick.gameObject.SetActive(true);
        _moveJoysick.gameObject.SetActive(false);
        _enemy.onDead += OnEnemyDead;
        _enemy.enabled = false;
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }
    protected override void Ready()
    {
        base.Ready();
        _ready = true; 
        _canvas.enabled = true;
    }
    private void OnEnemyDead(Character character)
    {
        _enemy.onDead -= OnEnemyDead;
        CompleteStep();
    }
    private void Update()
    {
        if (!_ready) return; 

        if(_aimJoysick.Vertical == 0 && _aimJoysick.Horizontal == 0)
        {
            if (!_canvas.enabled)
            {
                _canvas.enabled = true; 
            }
        }
        else
        {
            if (_canvas.enabled)
            {
                _canvas.enabled = false;
            }
        }
    }
}
