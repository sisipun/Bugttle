using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    private Slider slider;

    private int maxHealth;
    private int health;

    public bool IsDead => health <= 0;

    void Awake()
    {
        this.slider = GetComponent<Slider>();
    }

    public void Init(int health, Color color)
    {
        this.health = health;
        this.maxHealth = health;
        this.slider.maxValue = maxHealth;
        this.slider.value = health;
        this.fill.color = color;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }

        slider.value = health;
    }
}
