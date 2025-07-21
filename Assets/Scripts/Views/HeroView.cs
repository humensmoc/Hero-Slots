using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    [SerializeField] private LayerMask dropLayerMask;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text attackText;
    public Hero hero{get;private set;}
    public HoverInfoPanelData hoverInfoPanelData{get;private set;}

    public void Init(Hero hero){
        this.hero = hero;
        spriteRenderer.sprite = hero.Image;
        attackText.text = hero.Attack.ToString();
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Hero, spriteRenderer.sprite, hero.Name, hero.Description);
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

    void OnMouseDown(){
        if(!InteractionSystem.Instance.PlayerCanInteract())return;
        transform.position=MouseUtil.GetMousePostionInWorldSpace(-1);
        startPosition = transform.position;

        InteractionSystem.Instance.PlayerIsDragging = true;
    }

    void OnMouseDrag()
    {
        if(!InteractionSystem.Instance.PlayerCanInteract()) return;
        transform.position=MouseUtil.GetMousePostionInWorldSpace(-1);
    }

    void OnMouseUp()
    {
        if(!InteractionSystem.Instance.PlayerCanInteract()) return;
        InteractionSystem.Instance.PlayerIsDragging = false;
        transform.position=MouseUtil.GetMousePostionInWorldSpace(-1);

        if(Physics.Raycast(transform.position,Vector3.forward,out RaycastHit hit,10f,dropLayerMask))
        {
            HeroSlotView heroSlotView =InteractionSystem.Instance.EndTargeting(MouseUtil.GetMousePostionInWorldSpace(-1));

            HeroSystem.Instance.MoveToSlot(this,heroSlotView);
        }
        else
        {
            transform.position=startPosition;
        }
    }
}
