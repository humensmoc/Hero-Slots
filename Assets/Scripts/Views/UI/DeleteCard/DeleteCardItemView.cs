using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DeleteCardItemView : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Card card;
    public Image image;
    

    public void Init(Card card){
        this.card = card;
        image.sprite = ResourcesLoader.LoadCardSprite(card.CardData.CardNameEnum.ToString());
    }

    public void OnPointerClick(PointerEventData eventData){
        if(!DeleteCardPanelView.Instance.isDeleteCardMode)return;
        CardSystem.Instance.DeleteCardInDeck(card);
        DeleteCardPanelView.Instance.deleteCardItemViews.Remove(this);

        DeleteCardPanelView.Instance.isDeleteCardMode = false;
        DeleteCardPanelView.Instance.RefreshUI();
        
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData){

    }

    public void OnPointerExit(PointerEventData eventData){

    }
}