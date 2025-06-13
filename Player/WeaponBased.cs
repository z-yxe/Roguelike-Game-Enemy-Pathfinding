using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    protected Animator weaponAnimator;

    // Gets weapon animator
    protected virtual void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
    }

    // Defines attack method
    public abstract void Attack();
}
