using UnityEngine;

public abstract class MoveBehaviour 
{
    protected Player _player;
    protected Transform _playerTransform;

    protected const string ANIM_MOVE_VERTICAL = "Vertical";
    protected const string ANIM_MOVE_HORIZONTAL = "Horizontal";
    protected const float ROTATION_SPEED = 30f;
    protected const float MOVE_SPEED = 5f;
    protected const float AIM_MOVE_SPEED = 3.5f;

    public MoveBehaviour(Player player)
    {
        _player = player; 
        _playerTransform = _player.transform;
    }
    public abstract void Move(Vector3 moveVector, Vector3 aimVector);  
}