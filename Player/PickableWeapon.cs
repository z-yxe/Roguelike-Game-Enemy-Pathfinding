using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    public string weaponType;
    private SpriteRenderer spriteRenderer;

    // Sets sprite layer
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Character";
            spriteRenderer.sortingOrder = -1;
        }
    }

    // Updates weapon sprite
    public void SetWeaponSprite(Sprite weaponSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponSprite;
        }
    }
}
