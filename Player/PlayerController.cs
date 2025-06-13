using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform bodyTransform;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;

    // Cache components
    private Rigidbody2D rb;
    private Animator animator;
    private WeaponHolder weaponHolder;

    // Cache input references
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int HurtHash = Animator.StringToHash("Hurt");
    private static readonly int IsDeadHash = Animator.StringToHash("isDead");

    // Movement variables
    private Vector2 movement;
    private bool isMoving;
    private int currentHealth;
    private float lastNonZeroXMovement = 1f;

    // UI Environments
    public Button Pause_Button;
    public Health_Bar healthBar;
    public Equipment equipmentUIContainer;
    public GameOverScreen gameOverScreen;

    private int Score = 0;

    // Sets max health
    private void Start()
    {
        healthBar.setMaxHealth(maxHealth);
    }

    // Initializes components
    private void Awake()
    {
        InitializeComponents();
    }

    // Gets required components
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = bodyTransform.GetComponent<Animator>();
        weaponHolder = GetComponentInChildren<WeaponHolder>();
        currentHealth = maxHealth;
    }

    // Handles frame updates
    private void Update()
    {
        if (animator.GetBool(IsDeadHash)) return;

        HandleInput();
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause_Button.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!weaponHolder.HasWeapon())
            {
                equipmentUIContainer.ClearDisplay();
            }
            if (weaponHolder.HasWeapon() && weaponHolder.currentWeapon != null)
            {
                equipmentUIContainer.DisplayWeapon(weaponHolder.getWeaponName());
            }
        }
    }

    // Manages player input
    private void HandleInput()
    {
        HandleMovementInput();
        HandleWeaponInput();
        HandleCombatInput();
    }

    // Updates facing direction
    private void UpdateFacingDirection()
    {
        if (weaponHolder.HasWeapon())
        {
            Vector3 aimDirection = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
            SetCharacterFacing(aimDirection.x > 0);
        }
        else
        {
            if (movement.x != 0)
            {
                lastNonZeroXMovement = movement.x;
            }

            SetCharacterFacing(lastNonZeroXMovement > 0);
        }
    }

    // Handles movement input
    private void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        isMoving = movement != Vector2.zero;
    }

    // Flips character sprite
    private void SetCharacterFacing(bool faceRight)
    {
        Vector3 newScale = bodyTransform.localScale;
        newScale.x = faceRight ? 1 : -1;
        bodyTransform.localScale = newScale;
    }

    // Handles weapon input
    private void HandleWeaponInput()
    {
        if (Input.GetMouseButtonDown(1) && !weaponHolder.HasWeapon())
        {
            weaponHolder.TryPickupWeapon();
        }
        else if (Input.GetMouseButtonDown(1) && weaponHolder.HasWeapon())
        {
            weaponHolder.UnequipCurrentWeapon();
        }
    }

    // Handles combat input
    private void HandleCombatInput()
    {
        if (!weaponHolder.HasWeapon()) return;

        HandleAiming();
        if (Input.GetMouseButtonDown(0))
        {
            weaponHolder.currentWeapon.Attack();
        }
    }

    // Updates player animation
    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool(IsMovingHash, isMoving);
        UpdateFacingDirection();
    }

    // Handles weapon aiming
    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        weaponHolder.transform.rotation = Quaternion.Euler(0, 0, angle);
        weaponHolder.transform.localScale = new Vector3(
            1,
            (angle > 90 || angle < -90) ? -1f : 1f,
            1
        );
    }

    // Handles physics movement
    private void FixedUpdate()
    {
        if (animator.GetBool(IsDeadHash)) return;

        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    // Reduces player health
    public void TakeDamage(int damage)
    {
        if (animator.GetBool(IsDeadHash)) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        animator.SetTrigger(HurtHash);
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handles player death
    private void Die()
    {
        animator.SetBool(IsDeadHash, true);
        animator.SetBool(IsMovingHash, false);

        if (TryGetComponent<Collider2D>(out var collider))
        {
            collider.enabled = false;
        }

        gameOverScreen.ShowGameOverScreen();
        enabled = false;
    }

    // Updates game score
    public int updateScore()
    {
        gameOverScreen.updateScore(1);
        return Score += 1;
    }

    // Gets health percentage
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}
