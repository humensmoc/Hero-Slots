using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    Martial,
    Dart_Shooter,
    Dart_Wingman,
    Missile,
    Mayhem,
    adviser,
    bigAdviser,
    follower,
    bigFollower,
}

public enum CardRarity{
    Common,
    Rare,
    Epic,
    Legendary,
}



public static class CardLibrary
{

    public static List<CardData> cardDatas = new List<CardData>(){
        new CardData(CardName.Battery)
            .SetDescription("回合开始时：+1电能，处在角落时+3电能")
            .SetAttack(0)
            .SetElementType(ElementType.Element_Electricity)
            .SetCardRarity(CardRarity.Common)
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
                    return EffectComposer
                    .Sequential(
                        cardView.AddElectricity(1)
                    );
                }

                
            }),

        new CardData(CardName.Electric_Current)
            .SetDescription("攻击时：消耗1点电能,攻击后发射一颗弹射子弹，弹射次数等同于剩余电能数")
            .SetAttack(3)
            .SetElementType(ElementType.Element_Electricity)
            .SetCardRarity(CardRarity.Legendary)
            .SetOnInit((cardView) => {
            })
            .SetBullet(BulletName.Bullet_Bounce)
            .SetOnAttack((cardView) => {
                if(Model.Electricity <= 0)
                    return null;
                
                return EffectComposer
                    .Sequential(
                        cardView.SpendElectricity(1)
                    );
                
            })
            ,

        new CardData(CardName.Blood_Giver)
            .SetDescription("回合开始时：如果相邻单位中有红色单位，随机单位获得1鲜血宝石")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Epic)
            .SetOnInit((cardView) => {
                // Debug.Log("Card_Blood_Giver Init");
            })
            .SetOnTurnStart((cardView) => {
                
                bool hasFireNeighbor = false;
                List<CardView> neighborCardViews = CardSystem.Instance.GetAllNeighborCardView(cardView);

                if(neighborCardViews.Count == 0)
                    return null;

                foreach(CardView neighborCardView in neighborCardViews){
                    if(neighborCardView.card.ElementType == ElementType.Element_Fire){
                        hasFireNeighbor = true;
                        break;
                    }
                }
                
                if(hasFireNeighbor){
                    CardView targetCardView = CardSystem.Instance.GetRandomCardViewNotSelf(cardView);
                    if(targetCardView == null)
                        return null;
                    
                    bool isHasFireBoyInSameRow = false;
                    foreach(HeroView heroView in Model.HeroViews){
                        if(heroView.y == targetCardView.y&&heroView.hero.HeroType==HeroType.Fire_Boy){
                            isHasFireBoyInSameRow = true;
                            break;
                        }
                    }
                    
                    if(isHasFireBoyInSameRow){
                        return cardView.AddBloodGem(1,targetCardView,true);
                    }else{
                        return cardView.AddBloodGem(1,targetCardView);
                    }
                }
                return null;
            }),

        new CardData(CardName.Big_Blood_Giver)
            .SetDescription("倒计时3回合：本局内的<link=鲜血宝石>鲜血宝石</link>额外+1攻击力")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Rare)
            .SetOnInit((cardView) => {
                // Debug.Log("Card_Big_Blood_Giver Init");
            })
            .SetOnCountdownEnd(3, (cardView) => {
                return cardView.PowerUpBloodGem(1);
            }),

        new CardData(CardName.Water_Droplets)
            .SetDescription("攻击时：发射穿透3个敌人的子弹")
            .SetAttack(2)
            .SetElementType(ElementType.Element_Water)
            .SetBullet(BulletName.Bullet_Transparent)
            ,

        new CardData(CardName.Martial)
            .SetDescription("物理攻击")
            .SetAttack(2)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Common)
            .SetBullet(BulletName.Bullet_Martial)
            ,
        new CardData(CardName.Dart_Shooter)
            .SetDescription("物理攻击命中时，发射一个飞镖")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Common)
            .SetBullet(BulletName.Bullet_Normal)
            .SetOnInit((cardView) => {
                Func<EventInfo, IEnumerator> martialAttackHitEnemyAction = (eventInfo) => {
                    // return cardView.ShotDart(cardView,eventInfo.enemyView);
                    return cardView.AdditionalShot(
                        new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Dart))
                        ,eventInfo.enemyView);
                };
                
                EventSystem.Instance.AddAction(martialAttackHitEnemyAction,EventType.OnMartialAttackHitEnemy);

                cardView.card.CardData.OnLeave += () => {
                    EventSystem.Instance.RemoveAction(martialAttackHitEnemyAction,EventType.OnMartialAttackHitEnemy);
                };  
            })
            ,
        
        new CardData(CardName.Dart_Wingman)
            .SetDescription("Missile命中时，发射1个Mayhem")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Rare)
            .SetBullet(BulletName.Bullet_Normal)
            .SetOnInit((cardView) => {
                Func<EventInfo,IEnumerator> bulletHitEnemy=(eventInfo)=>{
                    // 如果子弹不是由卡牌发射的（比如由英雄发射），eventInfo.cardView可能为null
                    if(eventInfo.cardView == null || eventInfo.bulletView == null){
                        return null;
                    }
                    
                    if(eventInfo.bulletView.bullet.bulletData.BulletNameEnum==BulletName.Bullet_Missile){
                        Debug.Log("[Dart_Wingman]Missile Hit Enemy");
                        return cardView.AdditionalShot(new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Mayhem)));
                    }else{
                        return null;
                    }
                    
                };

                EventSystem.Instance.AddAction(bulletHitEnemy,EventType.OnBulletHitEnemy);

                cardView.card.CardData.OnLeave += () => {
                    EventSystem.Instance.RemoveAction(bulletHitEnemy,EventType.OnBulletHitEnemy);
                };
            })
            ,
        
        new CardData(CardName.Missile)
            .SetDescription("发射导弹，攻击最近的敌人")
            .SetAttack(3)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Common)
            .SetBullet(BulletName.Bullet_Missile)
            .SetOnAttack((cardView) => {
                if(Model.Electricity <= 0)
                    return null;
                
                return EffectComposer
                    .Sequential(
                        cardView.Shot(true,new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Missile)))
                    );
                
            })
            ,
        new CardData(CardName.Mayhem)
            .SetDescription("发射3个混乱子弹，攻击随机的敌人")
            .SetAttack(0)
            .SetElementType(ElementType.Element_Fire)
            .SetCardRarity(CardRarity.Common)
            .SetBullet(BulletName.Bullet_Mayhem)
            .SetOnAttack((cardView) => {
                return EffectComposer
                    .Sequential(
                        cardView.AdditionalShot(new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Mayhem))),
                        cardView.AdditionalShot(new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Mayhem))),
                        cardView.AdditionalShot(new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Mayhem)))
                    );
            })
            ,

        // new CardData(CardName.adviser)
        //     .SetDescription("==倒计时3，随机同色英雄攻击1次==")
        //     .SetAttack(1)
        //     .SetElementType(ElementType.Element_Water)
        //     .SetCardRarity(CardRarity.Common)
        //     .SetBullet(BulletName.Bullet_Normal)
        //     .SetOnCountdownEnd(3, (cardView) => {
        //         HeroView targetHeroView = null;

        //         List<HeroView> targetHeroViews = HeroSystem.Instance.heroViews.Where(heroView => heroView.hero.ElementType == cardView.card.ElementType).ToList();

        //         targetHeroView=targetHeroViews.Draw();
                
        //         if(targetHeroView == null){
        //             return null;
        //         }
                
        //         return targetHeroView.Shot();
        //     })
        //     ,
        new CardData(CardName.bigAdviser)
            .SetDescription("==攻击时，随机同色英雄攻击1次==")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Water)
            .SetCardRarity(CardRarity.Common)
            .SetBullet(BulletName.Bullet_Normal)
            .SetOnAttack((cardView) => {
                HeroView targetHeroView = null;

                List<HeroView> targetHeroViews = Model.HeroViews.Where(heroView => heroView.hero.ElementType == cardView.card.ElementType).ToList();

                targetHeroView=targetHeroViews.Draw();

                if(targetHeroView == null){
                    return null;
                }
                
                return targetHeroView.Shot();
            })
            ,
        // new CardData(CardName.follower)
        //     .SetDescription("同色英雄攻击时，发射1个什么子弹")
        //     .SetAttack(1)
        //     .SetElementType(ElementType.Element_Water)
        //     .SetCardRarity(CardRarity.Common)
        //     .SetBullet(BulletName.Bullet_Normal)
        //     .SetOnInit((cardView) => {
        //         Func<EventInfo, IEnumerator> heroAttackAction = (eventInfo) => {

        //             Debug.Log("[follower]Hero Attack: " + eventInfo.heroView.hero.ElementType + " " + cardView.card.ElementType);

        //             if(eventInfo.heroView.hero.ElementType == cardView.card.ElementType){
        //                 return cardView.AdditionalShot(new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Normal)));
        //             }else{
        //                 return HeroLibrary.EmptyCoroutine();
        //             }
        //         };

        //         EventSystem.Instance.AddAction(heroAttackAction,EventType.OnHeroAttack);

        //         cardView.card.CardData.OnLeave += () => {
        //             EventSystem.Instance.RemoveAction(heroAttackAction,EventType.OnHeroAttack);
        //         };
        //     })
        //     ,
        new CardData(CardName.bigFollower)
            .SetDescription("==同色英雄攻击时，发射1个价值等同于此英雄攻击力的子弹==")
            .SetAttack(1)
            .SetElementType(ElementType.Element_Water)
            .SetOnInit((cardView) => {
                Func<EventInfo, IEnumerator> heroAttackAction = (eventInfo) => {

                    // Debug.Log("[bigFollower]Hero Attack: " + eventInfo.heroView.hero.ElementType + " " + cardView.card.ElementType);

                    if(eventInfo.heroView.hero.ElementType == cardView.card.ElementType){
                        Bullet bullet = new Bullet(BulletLibrary.GetBulletDataByName(BulletName.Bullet_Normal));
                        bullet.Attack = eventInfo.heroView.hero.Attack;
                        return cardView.AdditionalShot(bullet);
                    }else{
                        return HeroLibrary.EmptyCoroutine();
                    }
                };

                EventSystem.Instance.AddAction(heroAttackAction,EventType.OnHeroAttack);

                cardView.card.CardData.OnLeave += () => {
                    EventSystem.Instance.RemoveAction(heroAttackAction,EventType.OnHeroAttack);
                };
            })
    };

    public static List<CardData> GetCardDatasByRarity(CardRarity rarity){
        return cardDatas.Where(cardData => cardData.CardRarity == rarity).ToList();
    }
}
