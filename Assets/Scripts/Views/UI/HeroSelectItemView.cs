using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HeroSelectItemView : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public HeroData data;
    public Image image;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text attackText;
    public Image elementImage;

    public void Init(HeroData data){
        this.data = data;
        image.sprite = ResourcesLoader.LoadHeroSprite(data.HeroType.ToString());
        nameText.text = data.HeroType.ToString();
        descriptionText.text = data.Description;
        attackText.text = data.Attack.ToString();
        elementImage.color = data.ElementType switch{
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
        HeroSelectSystem.Instance.SelectHero(data);
    }

    public void OnPointerEnter(PointerEventData eventData){
        transform.DOScale(1.1f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.DOScale(1f, 0.15f);
    }
}