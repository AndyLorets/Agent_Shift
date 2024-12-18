using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Character
{
    [SerializeField] private StateBase _startState;
    [SerializeField] private AttackState _attackState;
    [SerializeField] private PatroolState _patrolState;
    [SerializeField] private LureState _lureState;
    [SerializeField] private UnitInventory _unitInventory;
    [Space(5)]
    [SerializeField] private bool _isFootStepHear = true;
    [SerializeField] private bool _isRequestingAssistance = true;
    [SerializeField, Range(1, 3)] private int _visibleCountToAttack = 2;
    [SerializeField, Range(.1f, 1f)] private float _hitChance = .5f;
    [Space(5)]
    [SerializeField] private CharacterDialogue[] _dialogueOnHear;
    [SerializeField] private CharacterDialogue[] _dialogueOnVisibled;
    [SerializeField] private CharacterDialogue[] _dialogueOnAttack;
    [Space(5)]
    [SerializeField] private CanvasGroup _alertCanvas;
    [SerializeField] private Image _alertIcon;
    [SerializeField] private GameObject _miniMapIcon;
    [SerializeField] private VisionCone _visionCone;

    private Collider _collider;
    private StateMachine _stateMachine = new StateMachine();

    public System.Action<Vector3> onPlayerVisible;
    public NavMeshAgent agent { get; private set; }
    public Player player { get; private set; }
    public bool onAttack { get; private set; }

    private int _visibleCount;

    private const float CHECK_PLAYER_DURATION = .5f;

    public bool EnemyIsDetected => IsEnemyDetected() && !player.IsInvisibility;

#if UNITY_EDITOR
    private void OnValidate()
    {
        transform.name = $"Enemy {_startState.GetType()}";
    }
#endif
    private void OnEnable()
    {
        StartCoroutine(CheckPlayer());
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _collider = agent.GetComponent<Collider>();
    }
    private void Update()
    {
        if (agent.hasPath && agent.velocity.sqrMagnitude == 0f)
        {
            Vector3 destination = agent.destination;
            agent.ResetPath();
            agent.SetDestination(destination);
        }
        if (_stateMachine.currentState == _attackState && player.IsInvisibility)
        {
            ExitAllState();
            CancelInvoke(nameof(EnterPatrolState));
            Invoke(nameof(EnterPatrolState), 5f); 

            int r = Random.Range(0, _dialogueOnVisibled.Length);
            ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogueOnVisibled[r]);
        }
    }
    protected override void Construct()
    {
        base.Construct();

        player = ServiceLocator.GetService<Player>();
        if (_startState == _attackState)
            _attackState.isFirstState = true;
        _visionCone.Init(_visibleRange, _viewAngle);
        _stateMachine.ChangeState(_startState);
        Subscribe();
    }
    protected override void ConstructTargets()
    {
        _targets.Add(player);
    }
    private void Subscribe()
    {
        _lureState.onPlayerUnvisible += EnterPatrolState;
        player.onDead += OnPlayerDead;
        player.Skin.onFootStep += OnHearFootStep;
        GameManager.onGameWin += ExitAllState;
        EnemiesSubscrip(true);
    }
    private void Unsubscribe()
    {
        _lureState.onPlayerUnvisible -= EnterPatrolState;
        player.onDead -= OnPlayerDead;
        player.Skin.onFootStep -= OnHearFootStep;
        GameManager.onGameWin -= ExitAllState;
        EnemiesSubscrip(false);
    }
    private void EnemiesSubscrip(bool state)
    {
        foreach (Enemy enemy in _enemyManager.enemiesList)
        {
            if (enemy != this)
            {
                if (state)
                {
                    if (_isRequestingAssistance)
                        enemy.onPlayerVisible += OnRequestingAssistance;
                }
                else if (_isRequestingAssistance)
                    enemy.onPlayerVisible -= OnRequestingAssistance;
            }
        }
    }
    private IEnumerator CheckPlayer()
    {
        while (enabled)
        {
            if (_stateMachine.currentState != _attackState)
            {
                if (EnemyIsDetected)
                {
                    float dist = Vector3.Distance(transform.position, _targets[0].transform.position);
                    ExitAllState();       
                    _visibleCount++;

                    if (_alertCanvas.alpha != 1)
                    {
                        _alertCanvas.DOFade(1, 1f);
                        _alertIcon.DOFillAmount(1, CHECK_PLAYER_DURATION);
                        _visionCone.SetColor(VisionCone.ColorType.Visible);
                    }
                    if (_visibleCount == 1 && dist > 3)
                    {
                        int r = Random.Range(0, _dialogueOnVisibled.Length);
                        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogueOnVisibled[r]);
                    }
                    if (_visibleCount == _visibleCountToAttack || dist <= 3)
                    {
                        EnterAttackState();
                        _visibleCount = 0;
                        continue; 
                    }
                }
                else if (_stateMachine.currentState == null && _visibleCount > 0)
                {
                    EnterPatrolState();
                }
                else
                {
                    _visibleCount = 0;
                    if (_alertCanvas.alpha != 0)
                    {
                        _visionCone.SetColor(VisionCone.ColorType.Idle);
                        _alertIcon.DOFillAmount(0, CHECK_PLAYER_DURATION)
                            .OnComplete(() => _alertCanvas.alpha = 0);
                    }
                }
            }

            yield return new WaitForSeconds(CHECK_PLAYER_DURATION);
        }
    }

    private void OnPlayerDead(Character character)
    {
        if (character != player || !gameObject.activeSelf) return;

        EnterPatrolState();
    }
    private void OnRequestingAssistance(Vector3 pos)
    {
        if (!Alive || !gameObject.activeSelf) return; 

        float dist = Vector3.Distance(transform.position, pos);
        if (dist <= 12)
        {
            _stateMachine.ChangeState(_attackState);
            _visionCone.SetColor(VisionCone.ColorType.Attack);
        }
    }
    private void EnterAttackState()
    {
        if (_stateMachine.currentState == _attackState || player.IsInvisibility) return; 

        int r = Random.Range(0, _dialogueOnAttack.Length);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogueOnAttack[r], 1);
        _visionCone.SetColor(VisionCone.ColorType.Attack);
        _stateMachine.ChangeState(_attackState);
        onPlayerVisible?.Invoke(transform.position);
        onAttack = true;
        _alertCanvas.DOKill();
        _alertIcon.fillAmount = 0;
        _alertCanvas.alpha = 0;
    }
    private void EnterPatrolState()
    {
        if (_stateMachine.currentState == _patrolState) return;
        _visionCone.SetColor(VisionCone.ColorType.Idle);
        _stateMachine.ChangeState(_patrolState);
        onAttack = false;
    }
    public void EnterLureState(Vector3 pos)
    {
        if (!Alive) return; 

        if (_stateMachine.currentState == _attackState || _stateMachine.currentState == _lureState)
            return;

        int r = Random.Range(0, _dialogueOnHear.Length);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogueOnHear[r]);
        _lureState.SetLurePoint(pos);
        _stateMachine.ChangeState(_lureState);
        onAttack = false;
    }
    private void OnHearFootStep(Vector3 pos)
    {
        if (!_isFootStepHear) return;

        float dist = Vector3.Distance(transform.position, pos);
        if (dist > _visibleRange) return;

        if (_stateMachine.currentState == _attackState || _stateMachine.currentState == _lureState || !gameObject.activeSelf)
            return;

        int r = Random.Range(0, _dialogueOnHear.Length);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogueOnHear[r]);
        _lureState.SetLurePoint(pos);
        _stateMachine.ChangeState(_lureState);
        onAttack = false;
    }
    public override void TakeDamage(float value, bool headShoot)
    {
        base.TakeDamage(value, headShoot);
        if (_currentHP > 0)
            EnterAttackState();
    }
    public override bool IsEnemyDetected(out Vector3 pos, out bool headshoot)
    {
        bool isDetected = base.IsEnemyDetected(out pos, out headshoot);

        if (isDetected)
        {
            if (Random.value > _hitChance)
            {
                pos.y += Random.Range(_hitChance, 1f);
                pos.x += Random.Range(-1, -1);
            }
        }

        return isDetected;
    }
    public override void Dead(bool headShot)
    {
        base.Dead(headShot);

        Unsubscribe();
        ExitAllState();
        CancelInvoke();

        enabled = false;
        _collider.enabled = false;
        _visionCone.enabled = false;
        _visionCone.gameObject.SetActive(false);
        agent.enabled = false;
        _alertCanvas.DOKill(); 
        _alertCanvas.alpha = 0;
        _miniMapIcon.SetActive(false);

        if (_unitInventory.gameObject.activeSelf)
            _unitInventory.SpawnItem();
    }
    public void ExitAllState()
    {
        _stateMachine.ExitActiveState();
    }
    private void OnDestroy()
    {
        Unsubscribe(); 
    }
}
