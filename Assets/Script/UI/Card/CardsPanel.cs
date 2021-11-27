using UnityEngine;

public class CardsPanel : MonoBehaviour
{
    [SerializeField] private CardData[] cards;

    void Awake()
    {
        foreach (CardData cardData in cards)
        {
            Card card = Instantiate<GameObject>(cardData.Prefub).GetComponent<Card>();
            card.Init(cardData);
            card.transform.SetParent(gameObject.transform);
        }
    }
}
