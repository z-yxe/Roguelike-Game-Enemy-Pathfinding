using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public WeaponBase currentWeapon;
    private PickableWeapon nearbyWeapon;

    public GameObject pickableGunPrefab;
    public GameObject pickableSwordPrefab;

    public float pickupRange = 2f;
    public float dropDistance = 1f;
    public float dropDistanceDown = 0.5f;

    [SerializeField] private Transform bodyTransform;

    // Initializes weapon holder
    private void Start()
    {
        WeaponBase[] weapons = GetComponentsInChildren<WeaponBase>(true);
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        if (bodyTransform == null)
        {
            bodyTransform = transform.parent.Find("Body");
        }
    }

    // Detects nearby weapons
    private void OnTriggerEnter2D(Collider2D other)
    {
        PickableWeapon pickable = other.GetComponent<PickableWeapon>();
        if (pickable != null)
        {
            nearbyWeapon = pickable;
        }
    }

    // Clears nearby weapon
    private void OnTriggerExit2D(Collider2D other)
    {
        PickableWeapon pickable = other.GetComponent<PickableWeapon>();
        if (pickable != null && pickable == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

    // Attempts to pickup
    public void TryPickupWeapon()
    {
        if (nearbyWeapon != null)
        {
            EquipWeapon(nearbyWeapon.weaponType);
            Destroy(nearbyWeapon.gameObject);
            nearbyWeapon = null;
        }
    }

    // Activates specified weapon
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

    // Drops current weapon
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
                float direction = bodyTransform.localScale.x;

                Vector3 dropPosition = transform.position + new Vector3(
                    dropDistance * direction,
                    -dropDistanceDown,
                    0
                );

                GameObject droppedWeapon = Instantiate(prefabToSpawn, dropPosition, Quaternion.identity);

                SpriteRenderer spriteRenderer = droppedWeapon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = direction < 0;
                }
            }

            currentWeapon.gameObject.SetActive(false);
            currentWeapon = null;
        }
    }

    // Checks for weapon
    public bool HasWeapon()
    {
        return currentWeapon != null && currentWeapon.gameObject.activeSelf;
    }

    // Gets weapon's name
    public string getWeaponName()
    {
        return currentWeapon.weaponName;
    }
}
