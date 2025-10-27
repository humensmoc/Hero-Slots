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
        cardImage.sprite = ResourcesLoader.LoadCardSprite(cardData.CardNameEnum.ToString());
        cardName.text = cardData.Name;
        cardDescription.text = cardData.Description;
        cardAttack.text = cardData.Attack.ToString();
        elementImage.color = cardData.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,   
            ElementType.Element_Air => Color.white,
            ElementType.Element_Electricity => Color.yellow,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };
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