using UnityEngine;

public class MovementCondition : TutorialCondition
{
    [SerializeField] private Rigidbody _playerRB;

    private Canvas _canvas;

    private float _t; 
    public override void EnableCondition()
    {
        base.EnableCondition();

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }
    protected override void Ready()
    {
        base.Ready();
        _canvas.enabled = true;
    }
    private void Update()
    {
        if (_playerRB.velocity.magnitude > 0)
        {
            _t += Time.deltaTime;
            if (_t < 2f) return; 

            CompleteStep();
            gameObject.SetActive(false);    
        }
    }
}
