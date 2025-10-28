using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardName{
    Card_A,
    Card_B,
    Card_C,
    Electric_Current,
    Simple_Fire,
    Battery,
    Blood_Giver,
    Big_Blood_Giver,
    Water_Droplets,
}

public class CardEffect{
    public CardName CardNameEnum;
    public Action OnEnter;
    public Func<CardView, IEnumerator> OnTurnStart;
    public Action OnLeave;
    public Func<CardView, IEnumerator> OnAttack;
    public Action<CardView> OnInit;

    public CardEffect Clone(){
        CardEffect clone = new CardEffect(CardNameEnum);
        clone.OnInit = OnInit;
        clone.OnEnter = OnEnter;
        clone.OnLeave = OnLeave;
        clone.OnAttack = OnAttack;
        clone.OnTurnStart = OnTurnStart;
        return clone;
    }

    public CardEffect(CardName cardNameEnum){
        this.CardNameEnum = cardNameEnum;
    }

    public CardEffect SetOnInit(Action<CardView> action){
        OnInit = action;
        return this;
    }

    public CardEffect SetOnEnter(Action action){
        OnEnter = action;
        return this;
    }

    public CardEffect SetOnLeave(Action action){
        OnLeave = action;
        return this;
    }

    public CardEffect SetOnAttack(Func<CardView, IEnumerator> action){
        OnAttack = action;
        return this;
    }

    public CardEffect SetOnTurnStart(Func<CardView, IEnumerator> action){
        OnTurnStart = action;
        return this;
    }
}

public static class CardLibrary
{

    public static List<CardData> cardDatas = new List<CardData>(){
        new CardData(CardName.Battery)
            .SetDescription("+1电能，处在角落时+3电能")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Electricity)
            .SetOnInit((cardView) => {
                // Debug.Log("Card_A Init");
            })
            .SetOnTurnStart((cardView) => {
                if((cardView.x==0&&cardView.y==0)||(cardView.x==4&&cardView.y==0)||(cardView.x==0&&cardView.y==4)||(cardView.x==4&&cardView.y==4)){
                    return EffectComposer
                    .Sequential(
                        cardView.AddElectricity(1),
                        cardView.AddElectricity(1),
                        cardView.AddElectricity(1)
                    );
                }else{
                    return cardView.AddElectricity(1);
                }
            }),

        new CardData(CardName.Electric_Current)
            .SetDescription("消耗1点电能,攻击后发射一道电流")
            .SetAttack(3)
            .SetElementType(ElementType.Element_Electricity)
            .SetOnInit((cardView) => {
                Debug.Log("Card_Electric_Current Init");
            })
            .SetOnAttack((cardView) => {
                if(RuntimeEffectData.electricity <= 0)
                    return null;

                RuntimeEffectData.electricity--;
                return EffectComposer.Delayed(0.2f, cardView.AdditionalShot(
                    new Bullet(BulletLibrary.bulletDatas.Find(bulletData => bulletData.BulletNameEnum == BulletName.Bullet_Bounce).Clone())));
            }),

        new CardData(CardName.Blood_Giver)
            .SetDescription("回合开始时：随机单位获得1鲜血宝石")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetOnInit((cardView) => {
                // Debug.Log("Card_Blood_Giver Init");
            })
            .SetOnTurnStart((cardView) => {
                CardView targetCardView = CardSystem.Instance.GetRandomCardViewNotSelf(cardView);
                if(targetCardView == null)
                    return null;
                return cardView.AddBloodGem(3,targetCardView);
            }),

        new CardData(CardName.Big_Blood_Giver)
            .SetDescription("回合开始时：随机单位永久+1攻击")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetOnInit((cardView) => {
                // Debug.Log("Card_Big_Blood_Giver Init");
            })
            .SetOnTurnStart((cardView) => {
                CardView targetCardView = CardSystem.Instance.GetRandomCardViewNotSelf(cardView);
                if(targetCardView == null)
                    return null;
                return cardView.AddPermentAttack(1,targetCardView);
            }),

        new CardData(CardName.Water_Droplets)
            .SetDescription("发射穿透3个敌人的子弹")
            .SetAttack(2)
            .SetElementType(ElementType.Element_Water)
            .SetOnAttack((cardView) => {
                return cardView.AdditionalShot(new Bullet(BulletLibrary.bulletDatas.Find(bulletData => bulletData.BulletNameEnum == BulletName.Bullet_Transparent).Clone()));
            }),
    };
}
