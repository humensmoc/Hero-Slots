using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardView : MonoBehaviour
{
    public Card card{get ; private set;}


    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text attackText;

    public void Init(Card card){
        this.card = card;
        image.sprite = card.Image;
        attackText.text = card.Attack.ToString();
    }

    void OnMouseEnter()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1.1f, 0.15f);
        HoverInfoController.Instance.ShowHoverInfoPanel(new HoverInfoPanelData(HoverInfoPanelType.Card, image.sprite, card.Name, card.Description), gameObject);
    }

    void OnMouseExit()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1f, 0.15f);
        HoverInfoController.Instance.HideHoverInfoPanel();
    }
}
