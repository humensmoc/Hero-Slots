using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType{
    Hero_Alpha,
    Hero_Beta,
}

public class HeroEffect{
    public HeroType heroType;
    public Action<HeroView> OnInit;
    public Action<HeroView> OnDead;
    public Action<int,int> OnMove;
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
        clone.OnMove=OnMove;
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

}

public static class HeroLibrary
{
    // 辅助方法：返回空协程
    private static System.Collections.IEnumerator EmptyCoroutine(){
        yield return null;
    }
    
    public static List<HeroEffect> heroEffects = new List<HeroEffect>(){
            #region Hero_Alpha
            new HeroEffect(HeroType.Hero_Alpha)
                .SetInitEvent((thisHeroView) => {
                    EventSystem.Instance.AddActionFunction((eventInfo) => {
                        if(eventInfo.cardView.y == thisHeroView.y){
                            return thisHeroView.Shot();
                        }

                        return EmptyCoroutine();
                    },EventType.CardAttack);
                }),
            #endregion

            #region Hero_Beta
            new HeroEffect(HeroType.Hero_Beta)
            #endregion
        };
}
