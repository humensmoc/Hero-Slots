using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour
{
    public Hero hero{get;private set;}
    
    public int y;

    //interact
    public Vector3 dragOffset;
    [SerializeField] private LayerMask dropLayerMask;
    [SerializeField] private Vector3 startPosition;
    public HoverInfoPanelData hoverInfoPanelData{get;private set;}  
    public bool isMouseIn = false;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer elementImage;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private Image energyBar;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Image energyBarOutLine;



    

    public void Init(Hero hero,int y){
        this.hero = hero;
        spriteRenderer.sprite = hero.Image;
        attackText.text = hero.Attack.ToString();
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Hero, spriteRenderer.sprite, hero.Name, hero.Description);
        this.y = y;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();

        if(isMouseIn&&hero.isSkillCharged&&Input.GetMouseButtonDown(1)){
            CastSkill();
        }
    }

    public IEnumerator Shot(){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        yield return tw.WaitForCompletion();

        Tween tw2 =transform.DOShakePosition(0.1f,0.1f,10,90,false,true);
        yield return tw2.WaitForCompletion();
        
        Bullet bullet=new Bullet(GameInitializer.Instance.testBulletDatas[0]);
        bullet.Attack=hero.Attack;

        // Debug.Log("bullet.Attack:"+bullet.Attack);

        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);
    }

        public IEnumerator Shot(Bullet bullet){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        yield return tw.WaitForCompletion();

        Tween tw2 =transform.DOShakePosition(0.1f,0.1f,10,90,false,true);
        yield return tw2.WaitForCompletion();

        bullet.Attack=hero.Attack;

        // Debug.Log("bullet.Attack:"+bullet.Attack);

        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);
    }

    public IEnumerator GetCharged(){
        Tween tw =energyBar.transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            hero.Energy++;
            if(hero.Energy >= hero.MaxEnergy){
                ObjectPool.Instance.CreateJumpingText("Skill Ready !",transform.position,JumpingTextType.Normal);
            }else{
                ObjectPool.Instance.CreateJumpingText("Energy:"+hero.Energy.ToString() + "/" + hero.MaxEnergy.ToString(),transform.position,JumpingTextType.Normal);
            }
            energyBar.transform.DOScale(1f,0.075f);
        });
        yield return tw.WaitForCompletion();
    }

    public void CastSkill(){
        hero.heroData.HeroEffect.OnSkill?.Invoke(this);
        hero.Energy = 0;
    }

    public void UpdateUI(){
        if(hero == null) return;
        attackText.text = hero.Attack.ToString();
        energyBar.fillAmount = (float)hero.Energy / hero.MaxEnergy;
        energyText.text = hero.Energy.ToString() + "/" + hero.MaxEnergy.ToString();
        energyBar.color = hero.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,
            ElementType.Element_Air => Color.yellow,
            ElementType.Element_Light => Color.white,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };
        elementImage.color = hero.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,
            ElementType.Element_Air => Color.yellow,
            ElementType.Element_Light => Color.white,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };
        if(hero.isSkillCharged  ){
            energyBarOutLine.gameObject.SetActive(true);
        }
        else{
            energyBarOutLine.gameObject.SetActive(false);
        }
    }

    public void Remove(){
        hero.heroData.HeroEffect.OnDead?.Invoke(this);
    }



#region 鼠标事件
    void OnMouseEnter()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1.1f, 0.15f);
        HoverInfoController.Instance.MouseEnterTargetView(hoverInfoPanelData);
        isMouseIn=true;
    }

    void OnMouseExit()
    {
        if(!InteractionSystem.Instance.PlayerCanHover())return;
        transform.DOScale(1f, 0.15f);
        HoverInfoController.Instance.MouseExitTargetView(hoverInfoPanelData);
        isMouseIn=true;
    }

    void OnMouseDown(){
        
        if(!InteractionSystem.Instance.PlayerCanInteract())return;
        // if(!hero.isSkillCharged)return;

        dragOffset = transform.position - MouseUtil.GetMousePostionInWorldSpace(-1);
        transform.position=MouseUtil.GetMousePostionInWorldSpace(-1)+dragOffset;
        startPosition = transform.position;
        
        InteractionSystem.Instance.PlayerIsDragging = true;

        if(isMouseIn)HoverInfoController.Instance.MouseExitTargetView(hoverInfoPanelData);

        
    }

    void OnMouseDrag()
    {
        if(!InteractionSystem.Instance.PlayerCanInteract()) return;
        // if(!hero.isSkillCharged)return;


        transform.position = MouseUtil.GetMousePostionInWorldSpace(-1) + dragOffset;
    }

    void OnMouseUp()
    {
        if(!InteractionSystem.Instance.PlayerCanInteract()) return;
        // if(!hero.isSkillCharged)return;


        InteractionSystem.Instance.PlayerIsDragging = false;
        transform.position=MouseUtil.GetMousePostionInWorldSpace(-1)+dragOffset;
        dragOffset = Vector3.zero;

        Vector3 mousePosition = MouseUtil.GetMousePostionInWorldSpace(-1);
        if(Physics.Raycast(mousePosition,Vector3.forward,out RaycastHit hit,10f,dropLayerMask))
        {
            //移动到heroSlotView
            HeroSlotView heroSlotView =InteractionSystem.Instance.EndTargeting(mousePosition);
            // int fromIndex = HeroSystem.Instance.currentHeroSlotIndex;

            HeroSystem.Instance.MoveToSlot(this,heroSlotView);
            // int toIndex = HeroSystem.Instance.currentHeroSlotIndex;

            // //交换位置
            // Hero exchangeHero =HeroSystem.Instance.heroesInBattlefield[toIndex];
            // if(exchangeHero!=null){
            //     HeroView exchangeHeroView = HeroSystem.Instance.GetHeroView(exchangeHero);
            //     HeroSlotView exchangeHeroSlotView = HeroSystem.Instance.battlefieldView.heroSlotViews[fromIndex];

            //     HeroSystem.Instance.MoveToSlot(exchangeHeroView,exchangeHeroSlotView);
            // }
        }
        else
        {
            transform.position=startPosition;
        }

        if(isMouseIn)HoverInfoController.Instance.MouseEnterTargetView(hoverInfoPanelData);
    }

#endregion

}
