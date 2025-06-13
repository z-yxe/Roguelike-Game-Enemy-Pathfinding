using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Slider HealthSlider;

    // Sets maximum health
    public void setMaxHealth(int health)
    {
        HealthSlider.maxValue = health;
        HealthSlider.value = health;
    }

    // Updates current health
    public void setHealth(int amount)
    {
        HealthSlider.value = amount;
    }
}
