using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardCreator : Singleton<CardCreator>
{
    [SerializeField] GameObject cardViewPrefab;
    public CardView CreateCardView(Card card,Vector3 position,Quaternion rotation,int x,int y)
    {
        GameObject cardInstance = Instantiate(cardViewPrefab,position,rotation);
        cardInstance.transform.localScale = Vector3.zero;
        cardInstance.transform.DOScale(Vector3.one, 0.15f);
        CardView cardView = cardInstance.GetComponent<CardView>();
        cardView.Init(card,x,y);

        foreach(CardEffect cardEffect in CardLibrary.cardEffects){
            if(cardEffect.CardNameEnum == card.cardData.CardNameEnum){
                card.cardData.CardEffect = cardEffect.Clone();
            }
        }

        card.cardData.CardEffect.OnInit?.Invoke(cardView);
        return cardView;
    }
}
