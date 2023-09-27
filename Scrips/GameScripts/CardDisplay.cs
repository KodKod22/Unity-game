using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Image cardFront;
    public Image cardBack;

    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
        cardFront.sprite = card.icon;
    }

    public void CardCover(bool isCover)
    {
        cardBack.gameObject.SetActive(isCover);
    }
}
