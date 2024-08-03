using System.Collections;
using UnityEngine;

public class LureState : StateBase
{
    [SerializeField, Range(3, 7)] private float _waitTime = 3f;
    [SerializeField, Range(1, 3)] private float _visibleReactionDuration = 2;

    [SerializeField] private CharacterDialogue[] _CharacterDialogue;
    [SerializeField] private CharacterDialogue[] _CharacterDialogueVisible;

    private bool _isMove;
    private int _visibleCount;

    public System.Action onPlayerVisible;
    public System.Action onPlayerUnvisible;

    private const float visible_reaction_duration = 1f;
    private const float AGENT_MOVE_SPEED = 1.2f;

    private Enemy _enemy;
    private Vector3 _lurePoint;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        _isMove = _enemy.agent.velocity.sqrMagnitude > 0;
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);

        if (_enemy.agent.remainingDistance <= .5f && _isMove)
            StopMove();
    }
    public override void EnterState()
    {
        base.EnterState();

        _enemy.agent.SetDestination(transform.position);
        _visibleCount = 0;
        _enemy.agent.speed = AGENT_MOVE_SPEED;

        Invoke(nameof(SetMove), 1f);
        StartCoroutine(CheckPlayer());

        int r = Random.Range(0, _CharacterDialogue.Length);
        CharacterMessanger.instance.SetDialogueMessage(_enemy.icon, _CharacterDialogue[r].text, _CharacterDialogue[r].clip);
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();

        _isMove = false;
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);
    }
    private void SetMove()
    {
        if (_visibleCount > 0) return;

        _enemy.agent.SetDestination(_lurePoint);
    }
    private void StopMove()
    {
        if (_visibleCount > 0) return;

        _enemy.agent.SetDestination(transform.position);

        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(_waitTime);

        while (_visibleCount > 0)
            yield return null;

        onPlayerUnvisible?.Invoke();  
    }
    private IEnumerator CheckPlayer()
    {
        while (enabled)
        {
            if (_enemy.EnemyIsDetected)
            {
                StopMove();
                _visibleCount++;

                if (_visibleCount == 1)
                {
                    int r = Random.Range(0, _CharacterDialogueVisible.Length);
                    CharacterMessanger.instance.SetDialogueMessage(_enemy.icon, _CharacterDialogue[r].text, _CharacterDialogue[r].clip);
                }
            }
            else
                _visibleCount = 0;

            if (_visibleCount >= _visibleReactionDuration)
            {
                onPlayerVisible?.Invoke();
            }

            yield return new WaitForSeconds(visible_reaction_duration);

        }
    }
    public void SetLurePoint(Vector3 pos) => _lurePoint = pos;
}
  

