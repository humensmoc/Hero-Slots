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

    public IEnumerator Shot(){
        Tween tw =transform.DOScale(1.1f,0.075f).OnComplete(()=>{
            transform.DOScale(1f,0.075f);
        });
        // yield return tw.WaitForCompletion();
        yield return new WaitForSeconds(0.3f);

        yield return EventSystem.Instance.CheckEvent(new EventInfo(this,EventType.CardAttack));

        Bullet bullet=new Bullet(GameInitializer.Instance.testBulletData);
        bullet.Attack= attack;
        // Debug.Log("bullet.Attack:"+bullet.Attack);
        BulletView bulletView = BulletSystem.Instance.CreateBullet(
            bullet,
            transform.position,
            transform.rotation);

        BulletSystem.Instance.Shot(
            bulletView,
            transform.right*10);

        BattleSystem.Instance.OnCardAttack?.Invoke(CardSystem.Instance.battlefieldView.cardViews[x,y]);
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
