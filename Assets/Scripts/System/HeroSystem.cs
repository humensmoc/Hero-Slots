using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroSystem : Singleton<HeroSystem>
{
    public BattlefieldView battlefieldView;
    public List<Hero> heroes{get;private set;}=new();
    public List<HeroView> heroViews{get;private set;}=new();
    public int currentHeroSlotIndex{get;private set;}=0;

    public void Init(HeroData heroData){
        AddHero(new Hero(heroData));
        MoveToSlot(heroViews[0],battlefieldView.heroSlotViews[0]);
    }

    public void MoveToSlot(HeroView heroView,HeroSlotView heroSlotView){
        // 如果当前英雄槽位不为空，则将其缩放为1
        if(battlefieldView.currentHeroSlotView!=null)
        {
            battlefieldView.currentHeroSlotView.transform.DOScale(1f,0.15f);
        }
        battlefieldView.currentHeroSlotView = heroSlotView;
        battlefieldView.currentHeroSlotView.transform.DOScale(1.1f,0.15f);
        heroView.transform.DOMove(heroSlotView.transform.position,0.15f);

        heroView.y = battlefieldView.heroSlotViews.IndexOf(heroSlotView);

        // TestHeroEffect(heroView);

        //放大当前英雄槽位上的卡牌
        if(currentHeroSlotIndex==battlefieldView.heroSlotViews.IndexOf(heroSlotView)) return;
        for(int i=0;i<5;i++){
            if(battlefieldView.cardViews[i,currentHeroSlotIndex]!=null){
                battlefieldView.cardViews[i,currentHeroSlotIndex].transform.DOScale(1f,0.15f);
            }
        }

        Debug.Log("heroView.y: " + heroView.y+" , currentHeroSlotIndex: " + currentHeroSlotIndex);
        
        currentHeroSlotIndex=battlefieldView.heroSlotViews.IndexOf(heroSlotView);

        Debug.Log("currentHeroSlotIndex: " + currentHeroSlotIndex);
        for(int i=0;i<5;i++){
            if(battlefieldView.cardViews[i,currentHeroSlotIndex]!=null){
                battlefieldView.cardViews[i,currentHeroSlotIndex].transform.DOScale(1.1f,0.15f);
            }
        }
    }

    public void TestHeroEffect(HeroView heroView){
        List<CardView> cardViews = BattleSystem.Instance.GetCardViewByYIndex(heroView.y);
        foreach(CardView cardView in cardViews){
            cardView.attack += 1;
            cardView.UpdateUI();
            heroView.attack += 1;
            heroView.UpdateUI();
        }
    }

    public void AddHero(Hero hero){
        heroes.Add(hero);
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity,0);
        heroViews.Add(heroView);
        BattleSystem.Instance.OnCardAttack += heroView.hero.heroData.HeroEffect.OnCardAttack;
    }

    public void RemoveHero(Hero hero){
        heroes.Remove(hero);
        HeroView heroView = heroViews.Find(view => view.hero == hero);
        heroViews.Remove(heroView);
        Destroy(heroView.gameObject);
        BattleSystem.Instance.OnCardAttack -= heroView.hero.heroData.HeroEffect.OnCardAttack;
    }
}
