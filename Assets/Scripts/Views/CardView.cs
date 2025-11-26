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
    [SerializeField] private TMP_Text countdownText;

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

    public IEnumerator Shot(bool isAdditionalShot = false,Bullet bullet = null){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        // yield return tw.WaitForCompletion();
        yield return new WaitForSeconds(0.3f);
#region shot
        //如果攻击力小于等于0，不发射子弹       
        if(card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue>0){
            
            //子弹是否是特定类型
            if(bullet == null){

                bullet=new Bullet(BulletLibrary.bulletDatas.Find(bulletData => bulletData.BulletNameEnum == card.BulletNameEnum).Clone());
                bullet.Attack= card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue;
                
            }

            BulletView bulletView = BulletSystem.Instance.CreateBullet(
                bullet,
                transform.position,
                transform.rotation);

            BulletSystem.Instance.Shot(
                bulletView,
                transform.right*10);
        }
        
#endregion

#region trigger event
        //是否是额外攻击
        if(!isAdditionalShot){
            yield return EventSystem.Instance.CheckEvent(new EventInfo(this,EventType.CardAttack));

            // 主射击之后再执行OnAttack效果（比如附加射击）
            if(card.CardData.OnAttack != null){
                yield return card.CardData.OnAttack(this);
            }

            if(card.CardData.OnCountdownEnd != null){
                card.CardData.CurrentCountdown++;
                if(card.CardData.CurrentCountdown >= card.CardData.MaxCountdown){
                    yield return card.CardData.OnCountdownEnd(this);
                    card.CardData.CurrentCountdown = 0;
                }
            }

            yield return ChargeHero();
        }
#endregion
    }

    public IEnumerator AdditionalShot(Bullet bullet){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        // yield return tw.WaitForCompletion();
        yield return new WaitForSeconds(0.3f);

        if(bullet == null){
            bullet=new Bullet(BulletLibrary.bulletDatas[0].Clone());
            bullet.Attack= card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue;
        }else{
            bullet.Attack= card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue;
        }

        // Debug.Log("bullet.Attack:"+bullet.Attack);
        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);
    }

    public IEnumerator ChargeHero(){
        HeroView targetHeroView = null;
        foreach(HeroView heroView in HeroSystem.Instance.heroViews){
            if(heroView.y == y&&card.ElementType==heroView.hero.ElementType){
                targetHeroView = heroView;
            }
        }

        // 如果没有找到匹配的英雄，直接返回
        if(targetHeroView == null){
            yield break;
        }

        bool flyingTextCompleted = false;

        // 根据卡牌元素类型设置飞行文本类型
        FlyingTextType flyingTextType = FlyingTextType.AddBloodGem;
        switch(card.ElementType){
            case ElementType.Element_Fire:
                flyingTextType = FlyingTextType.ChargeRed;
                break;
            case ElementType.Element_Water:
                flyingTextType = FlyingTextType.ChargeBlue;
                break;
            case ElementType.Element_Electricity:
                flyingTextType = FlyingTextType.ChargeYellow;
                break;
        }
        

        // 创建飞行文本
        ObjectPool.Instance.CreateFlyingTextToTarget("ChargeHero",flyingTextType,transform.position,targetHeroView.transform.position,()=>{

            targetHeroView.hero.heroData.OnGetCharged?.Invoke(this,targetHeroView);
            targetHeroView.StartCoroutine(targetHeroView.GetCharged());
            flyingTextCompleted = true;
        },true);
        
        // 等待飞行文本完成
        yield return new WaitUntil(() => flyingTextCompleted);
        // yield return null;
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
        
        ObjectPool.Instance.CreateFlyingTextToTarget("+"+attack,FlyingTextType.AddBloodGem,transform.position,targetCardView.transform.position,()=>{
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
        
        ObjectPool.Instance.CreateFlyingTextToTarget("+"+attack,FlyingTextType.AddBloodGem,transform.position,targetCardView.transform.position,()=>{
            targetCardView.card.Attack+=attack;
            targetCardView.UpdateUI();

            flyingTextCompleted = true;
        });
        
        // 等待飞行文本完成
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator PowerUpBloodGem(int bloodGem){
        bool flyingTextCompleted = false;
        
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+bloodGem,
            FlyingTextType.PowerUpBloodGem,
            transform.position,
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.bloodGemValueText.transform.position),
            ()=>{
                RuntimeEffectData.bloodGemValue+=bloodGem;
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator AddBloodGem(int count ,CardView targetCardView,bool isPermanent=false){
        bool flyingTextCompleted = false;
        
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+count,
            FlyingTextType.AddBloodGem,
            transform.position,
            targetCardView.transform.position,
            ()=>{
                if(isPermanent){
                    targetCardView.card.Attack+=count*RuntimeEffectData.bloodGemValue;
                    targetCardView.UpdateUI();
                }else{
                    targetCardView.bloodGemCount+=count;
                    targetCardView.UpdateUI();
                }
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator AddElectricity(int electricity){
        bool flyingTextCompleted = false;
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "+"+electricity,
            FlyingTextType.AddElectricity,
            transform.position,
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.electricityText.transform.position),
            ()=>{
                RuntimeEffectData.electricity+=electricity;
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

    public IEnumerator SpendElectricity(int electricity){
        bool flyingTextCompleted = false;
        ObjectPool.Instance.CreateFlyingTextToTarget(
            "-"+electricity,
            FlyingTextType.AddElectricity,
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.electricityText.transform.position),
            transform.position,
            ()=>{
                RuntimeEffectData.electricity-=electricity;
                flyingTextCompleted = true;
            }
        );
        yield return new WaitUntil(() => flyingTextCompleted);
    }

#endregion

    public void UpdateUI(){
        if(card == null) return;
        attackText.text = (card.Attack+tempAdditionalAttack+bloodGemCount*RuntimeEffectData.bloodGemValue).ToString();
        if(card.CardData.MaxCountdown > 0){
            countdownText.text = card.CardData.CurrentCountdown.ToString() + "/" + card.CardData.MaxCountdown.ToString();
        }else{
            countdownText.text = "";
        }
        ElementImage.color = card.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,   
            ElementType.Element_Air => Color.white,
            ElementType.Element_Electricity => Color.yellow,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };
    }
    
    void OnMouseDown()
    {
        if(!DeleteCardPanelView.Instance.isDeleteCardMode)return;

        if(RuntimeEffectData.coin<Model.deleteCardCost){
            TipsController.Instance.ShowTips("Not enough coin");
            return;
        }

        InGameEconomySystem.Instance.SpendCoin(transform.position,Model.deleteCardCost);

        CardSystem.Instance.DeleteCardInBattleField(this);

        DeleteCardPanelView.Instance.isDeleteCardMode = false;
        DeleteCardPanelView.Instance.RefreshUI();
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
