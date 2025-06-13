using UnityEngine;

public class Gun : WeaponBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRate = 5f;
    float nextAttackTime = 0f;

    // Spawns bullet projectile
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition() - firePoint.position).normalized;
    }

    // Performs weapon attack
    public override void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            weaponAnimator.SetTrigger("Shoot");
            Shoot();
            nextAttackTime = Time.time + (1f / attackRate);
        }
    }
}
