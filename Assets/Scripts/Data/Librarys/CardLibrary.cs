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
                    return cardView.AddElectricity(1);
                }
            }),

        new CardData(CardName.Electric_Current)
            .SetDescription("攻击时：消耗1点电能,攻击后发射一颗弹射子弹，弹射次数等同于剩余电能数")
            .SetAttack(3)
            .SetElementType(ElementType.Element_Electricity)
            .SetCardRarity(CardRarity.Legendary)
            .SetOnInit((cardView) => {
                Debug.Log("Card_Electric_Current Init");
            })
            .SetBullet(BulletName.Bullet_Bounce)
            .SetOnAttack((cardView) => {
                if(RuntimeEffectData.electricity <= 0)
                    return null;
                
                return cardView.SpendElectricity(1);
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
                    foreach(HeroView heroView in HeroSystem.Instance.heroViews){
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
            .SetDescription("倒计时3回合：本局内的鲜血宝石额外+1攻击力")
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
    };
}
