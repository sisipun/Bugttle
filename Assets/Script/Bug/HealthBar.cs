using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefub;
    [SerializeField] private float pointDistance;

    private List<Image> points;
    private int maxHealth;
    private int health;

    public bool IsDead => health <= 0;
    public bool IsFull => health == maxHealth;
    public int Count => health;

    void Awake()
    {
        this.points = new List<Image>();
    }

    public void Init(int health, Color color)
    {
        this.health = health;
        this.maxHealth = health;

        for (int i = 0; i < this.maxHealth; i++)
        {
            if (i >= this.points.Count)
            {
                this.points.Add(Instantiate(pointPrefub, this.transform).GetComponent<Image>());
            }
            this.points[i].gameObject.SetActive(true);
            this.points[i].color = color;
        }
        for (int i = this.maxHealth; i < this.points.Count; i++)
        {
            this.points[i].gameObject.SetActive(false);
        }

        SetPointsPosition();
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdatePointsStatus();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }

        UpdatePointsStatus();
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    private void UpdatePointsStatus()
    {
        for (int i = 0; i < health; i++)
        {
            this.points[i].gameObject.SetActive(true);
        }
        for (int i = health; i < maxHealth; i++)
        {
            this.points[i].gameObject.SetActive(false);
        }
    }

    private void SetPointsPosition()
    {
        RectTransform transform = this.GetComponent<RectTransform>();
        float separatorDeltaX = transform.rect.width / maxHealth;

        RectTransform firstPointTransform = this.points[0].GetComponent<RectTransform>();
        RectTransform lastPointTransform = this.points[maxHealth - 1].GetComponent<RectTransform>();
        firstPointTransform.offsetMin = new Vector2(pointDistance / 2, firstPointTransform.offsetMin.y);
        lastPointTransform.offsetMax = new Vector2(-pointDistance / 2, lastPointTransform.offsetMax.y);

        for (int i = 0; i < this.maxHealth - 1; i++)
        {
            RectTransform leftPointTransform = this.points[i].GetComponent<RectTransform>();
            RectTransform rightPointTransform = this.points[i + 1].GetComponent<RectTransform>();
            float separatorX = (i + 1) * separatorDeltaX;
            leftPointTransform.offsetMax = new Vector2((separatorX - pointDistance / 2) - transform.rect.width, leftPointTransform.offsetMax.y);
            rightPointTransform.offsetMin = new Vector2(separatorX + pointDistance / 2, rightPointTransform.offsetMin.y);
        }
    }
}
