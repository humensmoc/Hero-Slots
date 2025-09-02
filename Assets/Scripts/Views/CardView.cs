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


    [SerializeField] private SpriteRenderer CardImage;
    [SerializeField] private SpriteRenderer ElementImage;
    [SerializeField] private TMP_Text attackText;

    public void Init(Card card,int x,int y){
        this.card = card;
        CardImage.sprite = card.Image;
        attackText.text = card.Attack.ToString();
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Card, CardImage.sprite, card.Name, card.Description);
        this.x = x;
        this.y = y;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public IEnumerator Shot(){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        // yield return tw.WaitForCompletion();
        yield return new WaitForSeconds(0.3f);

        yield return EventSystem.Instance.CheckEvent(new EventInfo(this,EventType.CardAttack));

        Bullet bullet=new Bullet(GameInitializer.Instance.testBulletDatas[0]);
        bullet.Attack= card.Attack;
        // Debug.Log("bullet.Attack:"+bullet.Attack);
        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);

        yield return ChargeHero();
    }

    public IEnumerator ChargeHero(){
        foreach(HeroView heroView in HeroSystem.Instance.heroViews){
            if(heroView.y == y&&card.ElementType==heroView.hero.ElementType){
                heroView.hero.heroData.HeroEffect.OnGetCharged?.Invoke(this,heroView);
                yield return heroView.GetCharged();
            }
        }
        yield return null;
    }

    public void UpdateUI(){
        if(card == null) return;
        attackText.text = card.Attack.ToString();
        ElementImage.color = card.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,   
            ElementType.Element_Air => Color.yellow,
            ElementType.Element_Light => Color.white,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };
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
