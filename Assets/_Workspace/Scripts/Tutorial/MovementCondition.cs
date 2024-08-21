using UnityEngine;

public class MovementCondition : TutorialCondition
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private GameObject[] _hideObjects; 
    private Canvas _canvas;

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
    }
    private void Update()
    {
        if (_playerRB.velocity.magnitude > 0)
        {
            _t += Time.deltaTime;
            if (_t < 2f) return; 

            CompleteStep();   
        }
    }
}
