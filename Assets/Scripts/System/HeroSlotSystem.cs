using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroSlotSystem : Singleton<HeroSlotSystem>
{
    public BattlefieldView battlefieldView;
    public int currentHeroSlotIndex{get;private set;}=0;

    public void MoveToSlot(HeroView heroView,HeroSlotView heroSlotView){
        // 如果当前英雄槽位不为空，则将其缩放为1
        if(battlefieldView.currentHeroSlotView!=null)
        {
            battlefieldView.currentHeroSlotView.transform.DOScale(1f,0.15f);
        }
        battlefieldView.currentHeroSlotView = heroSlotView;
        battlefieldView.currentHeroSlotView.transform.DOScale(1.1f,0.15f);
        heroView.transform.DOMove(heroSlotView.transform.position,0.15f);

        //放大当前英雄槽位上的卡牌
        if(currentHeroSlotIndex==battlefieldView.heroSlotViews.IndexOf(heroSlotView)) return;
        for(int i=0;i<5;i++){
            if(battlefieldView.cardViews[i,currentHeroSlotIndex]!=null){
                battlefieldView.cardViews[i,currentHeroSlotIndex].transform.DOScale(1f,0.15f);
            }
        }
        
        currentHeroSlotIndex=battlefieldView.heroSlotViews.IndexOf(heroSlotView);
        for(int i=0;i<5;i++){
            if(battlefieldView.cardViews[i,currentHeroSlotIndex]!=null){
                battlefieldView.cardViews[i,currentHeroSlotIndex].transform.DOScale(1.1f,0.15f);
            }
        }
    }
}
