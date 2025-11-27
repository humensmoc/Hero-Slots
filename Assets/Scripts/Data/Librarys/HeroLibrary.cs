using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType{
    Hero_Alpha,
    Water_Core,
    Electric_Man,
    Fire_Boy,
    Fire_Diarrhea,

}

public class HeroEffect{
    public HeroType heroType;
    public Action<HeroView> OnInit;
    public Action<HeroView> OnDead;
    public Action<int,int> OnMove;
    public Action<CardView,HeroView> OnGetCharged;
    public Action<HeroView> OnSkill;
    public Action<int,int> OnOtherHeroMove;
    public Action OnAttack;
    public Action<HeroView> OnOtherHeroAttack;
    public Action<CardView> OnCardGenerate;
    public Action<CardView> OnCardAttack;

    public HeroEffect(HeroType heroType){
        this.heroType = heroType;
    }

    public HeroEffect Clone(){
        HeroEffect clone = new HeroEffect(heroType);
        clone.OnInit=OnInit;
        clone.OnSkill=OnSkill;
        clone.OnMove=OnMove;
        clone.OnGetCharged=OnGetCharged;
        clone.OnOtherHeroMove=OnOtherHeroMove;
        clone.OnOtherHeroAttack=OnOtherHeroAttack;
        clone.OnCardGenerate=OnCardGenerate;
        clone.OnCardAttack=OnCardAttack;

        return clone;
    }

    public HeroEffect SetInitEvent(Action<HeroView> action){
        OnInit = action;
        return this;
    }

    public HeroEffect SetSkillEvent(Action<HeroView> action){
        OnSkill = action;
        return this;
    }

}

public static class HeroLibrary
{
    // 辅助方法：返回空协程
    public static System.Collections.IEnumerator EmptyCoroutine(){
        yield return null;
    }
    
    public static List<HeroData> heroDatas = new List<HeroData>(){
        // new HeroData(HeroType.Hero_Alpha)
        //     .SetDescription("同排同色单位攻击时，随机单位临时攻击+5，另一个随机单位永久攻击+1，并发射子弹")
        //     .SetAttack(3)
        //     .SetMaxEnergy(3)
        //     .SetElementType(ElementType.Element_Fire)
        //     .SetInitEvent((thisHeroView) => {
                    
        //         Func<EventInfo, IEnumerator> cardAttackAction = (eventInfo) => {
        //             if(eventInfo.cardView.y == thisHeroView.y&&eventInfo.cardView.card.ElementType==thisHeroView.hero.ElementType){

        //                 return EffectComposer.Sequential(
        //                     eventInfo.cardView.AddTempAttack(5, CardSystem.Instance.GetRandomCardViewNotSelf(eventInfo.cardView)),
        //                     eventInfo.cardView.AddPermentAttack(1, CardSystem.Instance.GetRandomCardViewNotSelf(eventInfo.cardView)),
        //                     thisHeroView.Shot()
        //                 );

        //             }else{
        //                 return EmptyCoroutine();
        //             }
        //         };
                
        //         EventSystem.Instance.AddAction(cardAttackAction,EventType.CardAttack);

        //         thisHeroView.hero.heroData.OnDead = (heroView) => {
        //             EventSystem.Instance.RemoveAction(cardAttackAction,EventType.CardAttack);
        //         };  
        //     })
        //     .SetSkillEvent((heroView) => {
        //         // Debug.Log("Hero_Alpha Skill");
        //         heroView.hero.value_1++;
        //         heroView.hero.Attack+=heroView.hero.value_1;
        //     }),

        new HeroData(HeroType.Water_Core)
            .SetDescription("技能：获得等同于回合数的攻击力，之后发射1颗能穿透3个敌人的子弹")
            .SetAttack(3)
            .SetMaxEnergy(3)
            .SetElementType(ElementType.Element_Water)
            .SetBullet(BulletName.Bullet_Transparent)
            .SetSkillEvent((heroView) => {
                heroView.hero.Attack+=TurnSystem.Instance.currentTurn/3+1;
                BulletData transparentBulletData = BulletLibrary.bulletDatas.Find(bulletData => bulletData.BulletNameEnum == BulletName.Bullet_Transparent);
                if(transparentBulletData != null){
                    heroView.StartCoroutine(heroView.Shot(new Bullet(transparentBulletData.Clone())));
                }
            }),

        new HeroData(HeroType.Electric_Man)
            .SetDescription("同排黄色单位攻击时：+1电能。技能：+5电能")
            .SetAttack(3)
            .SetMaxEnergy(3)
            .SetElementType(ElementType.Element_Electricity)
            .SetSkillEvent((heroView) => {
                RuntimeEffectData.electricity+=5;
            })
            .SetInitEvent((thisHeroView) => {
                    
                Func<EventInfo, IEnumerator> cardAttackAction = (eventInfo) => {
                    if(eventInfo.cardView.y == thisHeroView.y&&eventInfo.cardView.card.ElementType==thisHeroView.hero.ElementType){

                        return eventInfo.cardView.AddElectricity(1);

                    }else{
                        return EmptyCoroutine();
                    }
                };
                
                EventSystem.Instance.AddAction(cardAttackAction,EventType.CardAttack);

                thisHeroView.hero.heroData.OnDead = (heroView) => {
                    EventSystem.Instance.RemoveAction(cardAttackAction,EventType.CardAttack);
                };  
            }),

        new HeroData(HeroType.Fire_Boy)
            .SetDescription("同排单位接收的鲜血宝石带来的增益永久保留。")
            .SetAttack(3)
            .SetMaxEnergy(3)
            .SetElementType(ElementType.Element_Fire)
            //在卡牌中实现
            ,

        new HeroData(HeroType.Fire_Diarrhea)
            .SetDescription("同排红色单位攻击时：发射1颗子弹。技能：此英雄永久+1攻击力")
            .SetAttack(3)
            .SetMaxEnergy(3)
            .SetElementType(ElementType.Element_Fire)
            .SetInitEvent((thisHeroView) => {
                Func<EventInfo, IEnumerator> cardAttackAction = (eventInfo) => {
                    if(eventInfo.cardView.y == thisHeroView.y&&eventInfo.cardView.card.ElementType==thisHeroView.hero.ElementType){

                        return thisHeroView.Shot(true);

                    }else{
                        return EmptyCoroutine();
                    }
                };
                
                EventSystem.Instance.AddAction(cardAttackAction,EventType.CardAttack);

                thisHeroView.hero.heroData.OnDead = (heroView) => {
                    EventSystem.Instance.RemoveAction(cardAttackAction,EventType.CardAttack);
                };  
            })
            .SetSkillEvent((heroView) => {
                heroView.hero.Attack++;
            }),
    };
    
    
}
