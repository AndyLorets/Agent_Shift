using UnityEngine;

public class MovementCondition : TutorialCondition
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private GameObject[] _hideObjects;
    [SerializeField] private Joystick _moveJoysick; 

    private Canvas _canvas;
    private bool _ready; 
    private float _t; 
    public override void EnableCondition()
    {
        base.EnableCondition();

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;

        for (int i = 0; i < _hideObjects.Length; i++)
        {
            _hideObjects[i].gameObject.SetActive(false);
        }
    }
    protected override void Ready()
    {
        base.Ready();
        _canvas.enabled = true;
        _ready = true; 
    }
    private void Update()
    {
        if (!_ready) return;

        if (_moveJoysick.Vertical == 0 && _moveJoysick.Horizontal == 0)
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

        if (_playerRB.velocity.magnitude > 0)
        {
            _t += Time.deltaTime;
            if (_t < 2f) return; 

            CompleteStep();
            _canvas.enabled = false;
            _ready = false; 
        }
    }
}
