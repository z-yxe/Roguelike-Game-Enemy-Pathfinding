using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    public string weaponType; // "Gun" atau "Sword"
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Pastikan weapon selalu di belakang player
        if (spriteRenderer != null)
        {
            // Nilai negatif akan membuat object render di belakang object dengan nilai yang lebih tinggi
            spriteRenderer.sortingLayerName = "Character";
            spriteRenderer.sortingOrder = -1;
        }
    }

    public void SetWeaponSprite(Sprite weaponSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponSprite;
        }
    }
}