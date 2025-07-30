using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    public int y;
    [SerializeField] private LayerMask dropLayerMask;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text attackText;
    public Hero hero{get;private set;}
    public HoverInfoPanelData hoverInfoPanelData{get;private set;}
    public int attack;

    public void Init(Hero hero,int y){
        this.hero = hero;
        spriteRenderer.sprite = hero.Image;
        attack = hero.Attack;
        attackText.text = attack.ToString();
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Hero, spriteRenderer.sprite, hero.Name, hero.Description);
        this.y = y;
    }

    public IEnumerator Shot(){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        yield return tw.WaitForCompletion();

        Tween tw2 =transform.DOShakePosition(0.1f,0.1f,10,90,false,true);
        yield return tw2.WaitForCompletion();
        
        Bullet bullet=new Bullet(GameInitializer.Instance.testBulletData);
        bullet.Attack=attack;

        // Debug.Log("bullet.Attack:"+bullet.Attack);

        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);
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
