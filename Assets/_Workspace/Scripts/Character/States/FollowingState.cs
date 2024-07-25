using System.Collections;
using UnityEngine;

public class FollowingState : StateBase
{
    private const float WALK_SPEED = 1.4f;
    private const float RUN_SPEED = 5f;
    private const float RUN_DISTANCE = 7f;
    private const float WALK_DISTANCE = 3f;

    private Player _player;
    private Hostage _hostage;

    private bool _isMove; 

    protected override void Awake()
    {
        base.Awake();
        _hostage = GetComponent<Hostage>();
        _player = FindObjectOfType<Player>();
    }

    private IEnumerator Following()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame(); 

        while (true)
        {
            Vector3 playerPosition = _player.transform.position;
            float dist = Vector3.Distance(transform.position, playerPosition);
            bool isRun = _hostage.agent.velocity.sqrMagnitude > 0 && dist > RUN_DISTANCE;
            bool isWalk = _hostage.agent.velocity.sqrMagnitude > 0 && dist > WALK_DISTANCE && dist < RUN_DISTANCE;
            float speed = isRun ? RUN_SPEED : WALK_SPEED;

            _hostage.agent.speed = speed;
            _hostage.Animator.SetBool(ANIM_RUN, isRun);
            _hostage.Animator.SetBool(ANIM_WALK, isWalk);

            if (dist > WALK_DISTANCE)
            {
                _hostage.agent.SetDestination(playerPosition);
                _isMove = true;
            }
            else if (_isMove)
            {
                _isMove = false;
                _hostage.agent.SetDestination(transform.position);
            }

            yield return waitForEndOfFrame; 
        }
    }
    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(Following());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
