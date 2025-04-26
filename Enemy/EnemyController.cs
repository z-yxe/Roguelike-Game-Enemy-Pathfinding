using UnityEngine;
using System.Collections.Generic;

public class EnemyAIController : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float nextWaypointDistance = 0.1f;
    [SerializeField] private float pathUpdateRate = 0.5f;
    [SerializeField] private float gridCellSize = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Roaming Settings")]
    [SerializeField] private BoxCollider2D roamingCollider;
    [SerializeField] private float roamUpdateRate = 1f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float playerDetectionRange = 5f;

    [Header("Debug Settings")]
    [SerializeField] private bool showDebugAttack = true;
    [SerializeField] private Color debugColor = Color.white;
    [SerializeField] private bool showDebugPath = true;
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private float waypointSize = 0.3f;

    private enum EnemyState { Idle, Chasing, Attacking, Roaming }
    private EnemyState currentState;

    private Transform playerTransform;
    private Pathfinding pathfinding;
    private Animator animator;
    private Collider2D enemyCollider;

    private List<Vector3> currentPath;
    private int currentWaypointIndex;
    private Vector2 previousPosition;
    private Vector3 roamingTarget;
    private bool isRoaming = false;

    private float nextPathUpdateTime;
    private float nextAttackTime;

    private int currentHealth = 100;
    private bool isDead;
    private bool isProvoked = false;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HurtHash = Animator.StringToHash("Hurt");
    private static readonly int IsDeadHash = Animator.StringToHash("isDead");

    private void Awake()
    {
        InitializeComponents();
        InitializeAI();
    }

    private void InitializeComponents()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        previousPosition = transform.position;
    }

    private void InitializeAI()
    {
        const int GRID_SIZE = 100;
        Vector3 gridOrigin = new Vector3(0, 0);
        pathfinding = new Pathfinding(GRID_SIZE, GRID_SIZE, gridCellSize, gridOrigin, obstacleLayer);
        SetState(EnemyState.Idle);
    }

    private void Update()
    {
        if (isDead || playerTransform == null) return;

        UpdateMovementSpeed();
        UpdateState();
        HandleCurrentState();
    }

    private void UpdateMovementSpeed()
    {
        float currentSpeed = Vector2.Distance(transform.position, previousPosition) / Time.deltaTime;
        animator.SetFloat(SpeedHash, currentSpeed);
        previousPosition = transform.position;
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            SetState(EnemyState.Attacking);
        }
        else if (isProvoked || distanceToPlayer <= playerDetectionRange)
        {
            SetState(EnemyState.Chasing);
        }
        else
        {
            SetState(EnemyState.Roaming);
        }
    }

    private void HandleCurrentState()
    {
        switch (currentState)
        {
            case EnemyState.Chasing:
                HandleChasing();
                break;
            case EnemyState.Attacking:
                HandleAttacking();
                break;
            case EnemyState.Roaming:
                HandleRoaming();
                break;
        }
    }

    private void HandleChasing()
    {
        if (Time.time >= nextPathUpdateTime)
        {
            UpdatePath();
            nextPathUpdateTime = Time.time + pathUpdateRate;
        }

        FollowPath();
    }

    private void HandleRoaming()
    {
        if (!isRoaming || Time.time >= nextPathUpdateTime)
        {
            UpdateRoamingTarget();
            UpdatePath();
            nextPathUpdateTime = Time.time + roamUpdateRate;
        }

        FollowPath();
    }

    private void HandleAttacking()
    {
        if (Time.time >= nextAttackTime)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void UpdateRoamingTarget()
    {
        Vector2 colliderMin = roamingCollider.bounds.min;
        Vector2 colliderMax = roamingCollider.bounds.max;

        float randomX = Random.Range(colliderMin.x, colliderMax.x);
        float randomY = Random.Range(colliderMin.y, colliderMax.y);

        roamingTarget = new Vector3(randomX, randomY, 0);
        isRoaming = true;
    }

    private void UpdatePath()
    {
        if (currentState == EnemyState.Chasing)
        {
            currentPath = pathfinding.FindPath(transform.position, playerTransform.position);
        }
        else if (currentState == EnemyState.Roaming)
        {
            currentPath = pathfinding.FindPath(transform.position, roamingTarget);
        }
        currentWaypointIndex = 0;
    }

    private void FollowPath()
    {
        if (currentPath == null || currentWaypointIndex >= currentPath.Count)
        {
            animator.SetFloat(SpeedHash, 0f);
            return;
        }

        Vector3 currentWaypoint = currentPath[currentWaypointIndex];
        MoveTowardsTarget(currentWaypoint, moveSpeed);

        if (Vector3.Distance(transform.position, currentWaypoint) < nextWaypointDistance)
        {
            currentWaypointIndex++;
        }
    }

    private void MoveTowardsTarget(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        UpdateFacing(direction.x);
    }

    private void PerformAttack()
    {
        animator.SetTrigger(AttackHash);
    }

    public void ApplyAttackDamage()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            playerTransform.GetComponent<PlayerController>()?.TakeDamage(attackDamage);
        }
    }

    private void UpdateFacing(float directionX)
    {
        if (directionX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(directionX), 1, 1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        animator.SetTrigger(HurtHash);
        isProvoked = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        isProvoked = false;
        animator.SetBool(IsDeadHash, true);
        animator.SetFloat(SpeedHash, 0f);
        enemyCollider.enabled = false;
        enabled = false;

        Destroy(gameObject, 5f);
        playerTransform.GetComponent<PlayerController>().updateScore();
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (newState == EnemyState.Chasing)
        {
            UpdatePath();
            nextPathUpdateTime = Time.time + pathUpdateRate;
        }

        if (newState == EnemyState.Attacking)
        {
            animator.SetFloat(SpeedHash, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugAttack && !showDebugPath) return;

        if (showDebugAttack)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }

        if (currentPath != null && currentPath.Count > 0 &&showDebugPath)
        {
            Gizmos.color = pathColor;

            for (int i = currentWaypointIndex; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
                Gizmos.DrawWireSphere(currentPath[i], waypointSize);
            }

            Gizmos.DrawWireSphere(currentPath[currentPath.Count - 1], waypointSize);

            if (currentWaypointIndex < currentPath.Count)
            {
                Gizmos.DrawWireSphere(currentPath[currentWaypointIndex], waypointSize * 1.5f);
                Gizmos.DrawLine(transform.position, currentPath[currentWaypointIndex]);
            }
        }
    }
}