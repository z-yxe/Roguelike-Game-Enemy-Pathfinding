using UnityEngine;

public class Peluru : MonoBehaviour
{
    public float kecepatan = 10f;
    public float waktuHancur = 5f;
    public int damage = 5;
    public LayerMask targetLayer;  // Layer untuk enemy

    private Rigidbody2D rb;
    private Camera mainCam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        // Mendapatkan posisi mouse dalam world space
        Vector3 posisiMouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
        posisiMouse.z = 0;

        // Menghitung arah dan rotasi
        Vector2 arah = ((Vector2)(posisiMouse - transform.position)).normalized;
        float sudut = Mathf.Atan2(arah.y, arah.x) * Mathf.Rad2Deg;

        // Set rotasi dan kecepatan
        transform.rotation = Quaternion.Euler(0, 0, sudut);
        rb.linearVelocity = arah * kecepatan;

        // Set waktu hancur
        Destroy(gameObject, waktuHancur);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek jika menabrak enemy
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