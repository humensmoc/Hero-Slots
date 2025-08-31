using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CardSelectItemView : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public CardData cardData;
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    public TMP_Text cardAttack;
    public Image elementImage;

    public void Init(CardData cardData){
        this.cardData = cardData;
        cardImage.sprite = cardData.Image;
        cardName.text = cardData.Name;
        cardDescription.text = cardData.Description;
        cardAttack.text = cardData.Attack.ToString();
    }

    public void OnPointerClick(PointerEventData eventData){
        CardSelectSystem.Instance.SelectCard(cardData);
    }

    public void OnPointerEnter(PointerEventData eventData){
        transform.DOScale(1.1f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.DOScale(1f, 0.15f);
    }
}