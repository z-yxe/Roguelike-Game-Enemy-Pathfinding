using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    protected Animator weaponAnimator;

    protected virtual void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
    }

    public abstract void Attack();
}