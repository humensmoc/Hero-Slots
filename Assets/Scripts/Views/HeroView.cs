using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    [SerializeField] private LayerMask dropLayerMask;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Hero hero{get;private set;}

    public void Init(Hero hero){
        this.hero = hero;
        spriteRenderer.sprite = hero.Image;
    }

    void OnMouseEnter()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1.1f, 0.15f);
    }

    void OnMouseExit()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1f, 0.15f);
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

            HeroSlotSystem.Instance.MoveToSlot(this,heroSlotView);
        }
        else
        {
            transform.position=startPosition;
        }
    }
}
