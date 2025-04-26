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

    private void Start()
    {
        healthBar.setMaxHealth(maxHealth);
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = bodyTransform.GetComponent<Animator>();
        weaponHolder = GetComponentInChildren<WeaponHolder>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (animator.GetBool(IsDeadHash)) return; // Skip update if dead

        HandleInput();
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause_Button.GetComponent<Button>().onClick.Invoke();
        }

        // Bind (Mouse 2) untuk Display Item Ke equipmentUI - RAPI
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

    private void HandleInput()
    {
        HandleMovementInput();
        HandleWeaponInput();
        HandleCombatInput();
    }

    private void UpdateFacingDirection()
    {
        if (weaponHolder.HasWeapon())
        {
            // Logika facing untuk weapon tetap sama
            Vector3 aimDirection = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
            SetCharacterFacing(aimDirection.x > 0);
        }
        else
        {
            // Update last non-zero movement direction
            if (movement.x != 0)
            {
                lastNonZeroXMovement = movement.x;
            }

            // Use last movement direction for facing
            SetCharacterFacing(lastNonZeroXMovement > 0);
        }
    }

    private void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        isMoving = movement != Vector2.zero;
    }

    private void SetCharacterFacing(bool faceRight)
    {
        Vector3 newScale = bodyTransform.localScale;
        newScale.x = faceRight ? 1 : -1;
        bodyTransform.localScale = newScale;
    }

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

    private void HandleCombatInput()
    {
        if (!weaponHolder.HasWeapon()) return;

        HandleAiming();
        if (Input.GetMouseButtonDown(0))
        {
            weaponHolder.currentWeapon.Attack();
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool(IsMovingHash, isMoving);
        UpdateFacingDirection();
    }

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

    private void FixedUpdate()
    {
        if (animator.GetBool(IsDeadHash)) return; // Skip movement if dead

        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    public void TakeDamage(int damage)
    {
        if (animator.GetBool(IsDeadHash)) return; // Skip damage if already dead

        currentHealth = Mathf.Max(0, currentHealth - damage);
        animator.SetTrigger(HurtHash);
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

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

    public int updateScore()
    {
        gameOverScreen.updateScore(1);
        return Score += 1;
    }

    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}