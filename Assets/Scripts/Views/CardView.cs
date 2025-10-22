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

    public int tempAdditionalAttack;
    public int bloodGemCount;


    [SerializeField] private SpriteRenderer CardImage;
    [SerializeField] private SpriteRenderer ElementImage;
    [SerializeField] private TMP_Text attackText;

    public void Init(Card card,int x,int y){
        this.card = card;
        CardImage.sprite = ResourcesLoader.LoadCardSprite(card.CardData.CardNameEnum.ToString());
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

        // 如果有OnAttack效果，则作为协程执行
        if(card.CardData.OnAttack != null){
            yield return card.CardData.OnAttack(this);
        }

        yield return EventSystem.Instance.CheckEvent(new EventInfo(this,EventType.CardAttack));

        Bullet bullet=new Bullet(BulletLibrary.bulletDatas[0].Clone());
        bullet.Attack= card.Attack+tempAdditionalAttack;
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
                heroView.hero.heroData.OnGetCharged?.Invoke(this,heroView);
                yield return heroView.GetCharged();
            }
        }
        yield return null;
    }

#region Card Effect Methods 
    /// <summary>
    /// temporary buff
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="targetCardView"></param>
    /// <returns></returns>
    public IEnumerator AddTempAttack(int attack,CardView targetCardView){
        bool flyingTextCompleted = false;
        
        ObjectPool.Instance.CreateFlyingTextToTarget("+"+attack,FlyingTextType.BloodGem,transform.position,targetCardView.transform.position,()=>{
            targetCardView.tempAdditionalAttack+=attack;
            targetCardView.UpdateUI();

            flyingTextCompleted = true;
        });
        
        // 等待飞行文本完成
        yield return new WaitUntil(() => flyingTextCompleted);

    }

    /// <summary>
    /// permanent buff
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="targetCardView"></param>
    /// <returns></returns>
    public IEnumerator AddPermentAttack(int attack,CardView targetCardView){
        bool flyingTextCompleted = false;
        
        ObjectPool.Instance.CreateFlyingTextToTarget("+"+attack,FlyingTextType.BloodGem,transform.position,targetCardView.transform.position,()=>{
            targetCardView.card.Attack+=attack;
            targetCardView.UpdateUI();

            flyingTextCompleted = true;
        });
        
        // 等待飞行文本完成
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator PowerUpBloodGem(int bloodGem){
        bool flyingTextCompleted = false;
        RuntimeEffectData.bloodGemValue+=bloodGem;
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+bloodGem,
            FlyingTextType.BloodGem,
            transform.position,
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.bloodGemValueText.transform.position),
            ()=>{
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator AddBloodGem(int count ,CardView targetCardView){
        bool flyingTextCompleted = false;
        targetCardView.bloodGemCount+=count;
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+count,
            FlyingTextType.BloodGem,
            transform.position,
            targetCardView.transform.position,
            ()=>{
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator AddElectricity(int electricity){
        bool flyingTextCompleted = false;
        RuntimeEffectData.electricity+=electricity;
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+electricity,
            FlyingTextType.AddElectricity,
            transform.position,
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.electricityText.transform.position),
            ()=>{
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

#endregion

    public void UpdateUI(){
        if(card == null) return;
        attackText.text = (card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue).ToString();
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
