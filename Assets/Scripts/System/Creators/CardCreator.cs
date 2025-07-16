using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardCreator : Singleton<CardCreator>
{
    [SerializeField] GameObject cardViewPrefab;
    public CardView CreateCardView(Card card,Vector3 position,Quaternion rotation)
    {
        GameObject cardInstance = Instantiate(cardViewPrefab,position,rotation);
        cardInstance.transform.localScale = Vector3.zero;
        cardInstance.transform.DOScale(Vector3.one, 0.15f);
        CardView cardView = cardInstance.GetComponent<CardView>();
        cardView.Init(card);
        return cardView;
    }
}
