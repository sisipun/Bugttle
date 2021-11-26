using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image image;

    public UnityAction<Card> OnClick;

    private CardData data;

    public void Init(CardData data)
    {
        this.data = data;
        this.image.sprite = data.Image;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            OnClick(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}