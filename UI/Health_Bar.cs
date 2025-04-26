using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    // ( RAPI )

    public Slider HealthSlider;

    // Set max health ke slider value
    public void setMaxHealth(int health)
    {
        HealthSlider.maxValue = health;
        HealthSlider.value = health;
    }

    // Untuk Update value di slider
    public void setHealth(int amount)
    {
        HealthSlider.value = amount;
    }
}
