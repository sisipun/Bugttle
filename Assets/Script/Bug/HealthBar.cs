using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    private Slider slider;

    private int maxHealth;
    private int health;

    public bool IsDead => health <= 0;
    public bool IsFull => health == maxHealth;
    public int Count => health;

    void Awake()
    {
        this.slider = GetComponent<Slider>();
    }

    void Update()
    {
        this.slider.value = health;
    }

    public void Init(int health, Color color)
    {
        this.health = health;
        this.maxHealth = health;
        this.slider.maxValue = maxHealth;
        this.slider.value = health;
        this.fill.color = color;
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        slider.value = health;
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }

        slider.value = health;
    }
}
