using UnityEngine;

public class AimMoveBehaviour : MoveBehaviour
{
    public AimMoveBehaviour(Player player) : base(player)
    {
        string weaponName = _player.CurrentWeaponName;
        _player.Animator.CrossFade($"{weaponName}_Aim", .1f);
    }

    public override void Move(Vector3 movementInput, Vector3 aimVector)
    {
        Vector3 movementDirection = _playerTransform.InverseTransformDirection(movementInput);
        movementDirection.y = 0; 

        _player.Animator.SetFloat(ANIM_MOVE_HORIZONTAL, movementDirection.x * .85f);
        _player.Animator.SetFloat(ANIM_MOVE_VERTICAL, movementDirection.z * .85f);

        _playerTransform.position += movementInput * AIM_MOVE_SPEED * Time.deltaTime;

        Vector3 directionToTarget = new Vector3(aimVector.x, 0, aimVector.z);
        directionToTarget.Normalize(); 

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, targetRotation, Time.deltaTime * ROTATION_SPEED);
        }
    }
}
