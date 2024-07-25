using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRigController : MonoBehaviour
{
    [SerializeField] private Transform _aimTarget;

    private Vector3 _aimTargetLocalStartPos;
    private WeaponBase _weapon; 
    private Rig _rig;
    private bool _active; 
    public void Construct(WeaponBase weapon)
    {
        _weapon = weapon;
        _weapon.onStartReload += DeactiveRig;
        _weapon.onEndReload += ActiveRig;
    }
    private void Awake()
    {
        _rig = GetComponent<Rig>();
        _aimTargetLocalStartPos = _aimTarget.localPosition;
    }
    private void Update()
    {
        float value = _active ? 1 : 0;
        _rig.weight = Mathf.Lerp(_rig.weight, value, Time.deltaTime * 15f);
    }
    public void ActiveRig(int agr1 = 0, int agr2 = 0)
    {
        _active = true; 
    }
    public void DeactiveRig()
    {
        _active = false;
        _aimTarget.localPosition = _aimTargetLocalStartPos;
    }
    public void SetAimTargetPos(Vector3 pos = new Vector3())
    {
        if (pos != Vector3.zero)
            _aimTarget.position = pos + Vector3.up;
        else
            _aimTarget.localPosition = _aimTargetLocalStartPos;
    }
    private void OnDestroy()
    {
        _weapon.onStartReload -= DeactiveRig;
        _weapon.onEndReload -= ActiveRig;
    }
}
