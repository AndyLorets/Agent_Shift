using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRigController : MonoBehaviour
{
    private Weapon _weapon; 
    private Rig _rig;
    private bool _active; 
    public void Construct(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.onStartReload += DeactiveRig;
        _weapon.onEndReload += ActiveRig;
    }
    private void Awake()
    {
        _rig = GetComponent<Rig>(); 
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
    }
    private void OnDestroy()
    {
        _weapon.onStartReload -= DeactiveRig;
        _weapon.onEndReload -= ActiveRig;
    }
}
