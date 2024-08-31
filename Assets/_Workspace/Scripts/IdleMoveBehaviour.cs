using UnityEngine;

public class IdleMoveBehaviour : MoveBehaviour 
{
    public IdleMoveBehaviour(Player player) : base(player)
    {
        string weaponName = _player.CurrentWeaponName;
        _player.Animator.CrossFade($"{weaponName}_Move", .1f);
    }
    public override void Move(Vector3 moveVector, Vector3 aimVector)
    {
        _player.Animator.SetFloat(ANIM_MOVE_VERTICAL, moveVector.magnitude);
        _player.transform.position += moveVector * MOVE_SPEED * Time.deltaTime; 

        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0f)
        {
            Vector3 direct = Vector3.RotateTowards(_playerTransform.forward, moveVector, MOVE_SPEED, 0.0f);
            Quaternion targetRotation = Quaternion.LookRotation(direct);
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, targetRotation, Time.deltaTime * ROTATION_SPEED);
        }
    }
}