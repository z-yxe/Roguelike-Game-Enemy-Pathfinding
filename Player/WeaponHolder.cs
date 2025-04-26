using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public WeaponBase currentWeapon;
    public float pickupRange = 2f;
    private PickableWeapon nearbyWeapon;

    // Referensi ke prefab senjata yang bisa dipungut
    public GameObject pickableGunPrefab;
    public GameObject pickableSwordPrefab;

    // Konfigurasi drop position
    public float dropDistance = 1f;     // Jarak horizontal drop
    public float dropDistanceDown = 0.5f;   // Jarak ke bawah

    [SerializeField] private Transform bodyTransform; // Referensi ke transform body player

    private void Start()
    {
        // Nonaktifkan semua senjata pada awal game
        WeaponBase[] weapons = GetComponentsInChildren<WeaponBase>(true);
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        // Jika bodyTransform belum di-assign, coba cari dari parent
        if (bodyTransform == null)
        {
            bodyTransform = transform.parent.Find("Body");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        PickableWeapon pickable = other.GetComponent<PickableWeapon>();
        if (pickable != null)
        {
            nearbyWeapon = pickable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PickableWeapon pickable = other.GetComponent<PickableWeapon>();
        if (pickable != null && pickable == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

    public void TryPickupWeapon()
    {
        if (nearbyWeapon != null)
        {
            EquipWeapon(nearbyWeapon.weaponType);
            Destroy(nearbyWeapon.gameObject);
            nearbyWeapon = null;
        }
    }

    public void EquipWeapon(string weaponName)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        WeaponBase[] weapons = GetComponentsInChildren<WeaponBase>(true);
        foreach (var weapon in weapons)
        {
            if (weapon.weaponName.Equals(weaponName, System.StringComparison.OrdinalIgnoreCase))
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon;
                break;
            }
        }
    }

    public void UnequipCurrentWeapon()
    {
        if (currentWeapon != null)
        {
            GameObject prefabToSpawn = null;
            if (currentWeapon.weaponName.Equals("Gun", System.StringComparison.OrdinalIgnoreCase))
            {
                prefabToSpawn = pickableGunPrefab;
            }
            else if (currentWeapon.weaponName.Equals("Sword", System.StringComparison.OrdinalIgnoreCase))
            {
                prefabToSpawn = pickableSwordPrefab;
            }

            if (prefabToSpawn != null && bodyTransform != null)
            {
                // Tentukan arah berdasarkan scale x dari body
                float direction = bodyTransform.localScale.x;

                // Hitung posisi drop
                Vector3 dropPosition = transform.position + new Vector3(
                    dropDistance * direction, // Horizontal sesuai arah hadap
                    -dropDistanceDown,        // Ke bawah
                    0                         // Z tetap sama
                );

                // Spawn weapon yang di-drop
                GameObject droppedWeapon = Instantiate(prefabToSpawn, dropPosition, Quaternion.identity);

                // Set sorting order
                SpriteRenderer spriteRenderer = droppedWeapon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {

                    // Optional: flip sprite sesuai arah hadap player
                    spriteRenderer.flipX = direction < 0;
                }
            }

            currentWeapon.gameObject.SetActive(false);
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null && currentWeapon.gameObject.activeSelf;
    }

    public string getWeaponName()
    {
        return currentWeapon.weaponName;
    }
}