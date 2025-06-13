using UnityEngine;

public class Peluru : MonoBehaviour
{
    public float kecepatan = 10f;
    public float waktuHancur = 5f;
    public int damage = 5;
    public LayerMask targetLayer;

    private Rigidbody2D rb;
    private Camera mainCam;

    // Gets necessary components
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    // Initializes bullet behavior
    private void Start()
    {
        Vector3 posisiMouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
        posisiMouse.z = 0;

        Vector2 arah = ((Vector2)(posisiMouse - transform.position)).normalized;
        float sudut = Mathf.Atan2(arah.y, arah.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, sudut);
        rb.linearVelocity = arah * kecepatan;

        Destroy(gameObject, waktuHancur);
    }

    // Handles bullet collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            EnemyAIController enemyController = collision.gameObject.GetComponent<EnemyAIController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
