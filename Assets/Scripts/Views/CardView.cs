using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardView : MonoBehaviour
{
    public Card card{get ; private set;}
    public HoverInfoPanelData hoverInfoPanelData{get;private set;}
    public int x;
    public int y;
    public int attack;


    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text attackText;

    public void Init(Card card,int x,int y){
        this.card = card;
        image.sprite = card.Image;
        attack = card.Attack;
        attackText.text = attack.ToString();
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Card, image.sprite, card.Name, card.Description);
        this.x = x;
        this.y = y;
    }

    public void UpdateUI(){
        attackText.text = attack.ToString();
    }

    void OnMouseEnter()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1.1f, 0.15f);
        HoverInfoController.Instance.MouseEnterTargetView(hoverInfoPanelData);
    }

    void OnMouseExit()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1f, 0.15f);
        HoverInfoController.Instance.MouseExitTargetView(hoverInfoPanelData);
    }
}
