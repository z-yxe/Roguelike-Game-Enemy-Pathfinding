using UnityEngine;
using System.Collections;

public class Sword : WeaponBase
{
    public LayerMask enemyLayers;
    public Transform attackPoint;
    
    public float attackRange = 0.5f;
    public float attackRate = 5f;
    public int damage = 10;
    
    float nextAttackTime = 0f;

    // Performs melee attack
    public override void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            weaponAnimator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyAIController>().TakeDamage(damage);
            }

            nextAttackTime = Time.time + (1f / attackRate);
        }
    }

    // Visualizes attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
