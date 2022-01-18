using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private GameObject separatorPrefub;
    [SerializeField] private float separatorSize;

    private Slider slider;
    private List<GameObject> separators;

    private int maxHealth;
    private int health;

    public bool IsDead => health <= 0;
    public bool IsFull => health == maxHealth;
    public int Count => health;

    void Awake()
    {
        this.separators = new List<GameObject>();
        this.slider = GetComponent<Slider>();
    }

    public void Init(int health, Color color)
    {
        this.health = health;
        this.maxHealth = health;
        this.slider.maxValue = maxHealth;
        this.slider.value = health;
        this.fill.color = color;

        for (int i = 0; i < this.maxHealth - 1; i++)
        {
            if (i >= this.separators.Count)
            {
                this.separators.Add(Instantiate(separatorPrefub, this.transform));
            }
            this.separators[i].SetActive(true);
        }
        for (int i = this.maxHealth - 1; i < this.separators.Count; i++)
        {
            this.separators[i].SetActive(false);
        }

        RectTransform transform = this.GetComponent<RectTransform>();
        float separatorDeltaX = transform.rect.width / maxHealth;
        for (int i = 0; i < this.maxHealth - 1; i++)
        {
            RectTransform separatorTransform = this.separators[i].GetComponent<RectTransform>();
            float separatorX = (i + 1) * separatorDeltaX;
            separatorTransform.offsetMin = new Vector2(separatorX, separatorTransform.offsetMin.y);
            separatorTransform.offsetMax = new Vector2((separatorX + separatorSize) - transform.rect.width, separatorTransform.offsetMax.y);
        }
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
