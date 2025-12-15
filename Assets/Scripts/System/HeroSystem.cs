using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Burst.CompilerServices;

public class HeroSystem : Singleton<HeroSystem>
{
    public BattlefieldView battlefieldView;
    public List<Hero> heroesInDeck{get;private set;}=new();
    public List<Hero> heroesInBattlefield{get;private set;}=new List<Hero>(){null, null, null, null, null};
    public List<HeroView> heroViews{get;private set;}=new();
    public int currentHeroSlotIndex{get;private set;}=0;
    public float heroPositionInterval ;
    public Transform heroParent;

    public void Init(List<HeroData> heroDatas){

        heroesInDeck.Add(new Hero(heroDatas[Random.Range(0, heroDatas.Count)]));

        StartCoroutine(DrawAllHero());
        
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

        // // 放大当前英雄槽位上的卡牌
        // if(currentHeroSlotIndex==battlefieldView.heroSlotViews.IndexOf(heroSlotView)) return;
        // for(int i=0;i<5;i++){
        //     if(battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex]!=null){
        //         battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex].transform.DOScale(1f,0.15f);
        //     }
        // }

        Debug.Log("heroView.y: " + heroView.y+" , currentHeroSlotIndex: " + currentHeroSlotIndex);
        
        currentHeroSlotIndex=battlefieldView.heroSlotViews.IndexOf(heroSlotView);

        // Debug.Log("currentHeroSlotIndex: " + currentHeroSlotIndex);
        // for(int i=0;i<5;i++){
        //     if(battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex]!=null){
        //         battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex].transform.DOScale(1.1f,0.15f);
        //     }
        // }
    }
    public void AddHero(Hero hero){
        heroesInDeck.Add(hero);
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity,0);
        heroViews.Add(heroView);
    }

    public void RemoveHero(Hero hero){
        heroesInDeck.Remove(hero);
        HeroView heroView = heroViews.Find(view => view.hero == hero);
        heroViews.Remove(heroView);
        Destroy(heroView.gameObject);
    }

    public IEnumerator DrawHero(){
        // 检查战场是否有空位
        List<int> emptyIndexs = new List<int>();
        for(int i = 0; i < heroesInBattlefield.Count; i++){
            if(heroesInBattlefield[i] == null)
                emptyIndexs.Add(i);
        }
        if(emptyIndexs.Count==0)
            yield break;

        // 获取空位的索引
        int randomY=emptyIndexs[Random.Range(0, emptyIndexs.Count)];

        Hero hero= heroesInDeck[Random.Range(0, heroesInDeck.Count)];
        heroesInDeck.Remove(hero);
        heroesInBattlefield[randomY] = hero;

        // 创建英雄视图
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity,randomY);
        heroViews.Add(heroView);

        Tween tween = heroView.transform.DOLocalMove(heroParent.position + new Vector3(0, randomY * heroPositionInterval, 0), 0.15f);
        yield return tween.WaitForCompletion();
    }

    public IEnumerator DiscardHero(Hero hero){

        heroesInDeck.Add(hero);
        // 找到英雄在战场中的索引并设置为null
        int index = heroesInBattlefield.IndexOf(hero);
        if(index >= 0){
            heroesInBattlefield[index] = null;
        }
        HeroView heroView = heroViews.Find(view => view.hero == hero);
        heroView.Remove();
        
        heroViews.Remove(heroView);
        
        Tween tween = heroView.transform.DOScale(Vector3.zero,0.15f).OnComplete(()=>{
            Destroy(heroView.gameObject);
        });
        yield return tween.WaitForCompletion();
    }

    public IEnumerator DrawAllHero(){
        while(heroesInDeck.Count > 0){
            yield return DrawHero();
        }
    }

    public IEnumerator DiscardAllHero(){
        for(int i = 0; i < heroesInBattlefield.Count; i++){
            if(heroesInBattlefield[i] != null){
                yield return DiscardHero(heroesInBattlefield[i]);
            }
        }
    }

    public HeroView GetHeroView(Hero hero){
        return heroViews.Find(view => view.hero == hero);
    }

    public void Reset()
    {
        StopAllCoroutines();
        heroesInDeck.Clear();
        heroesInBattlefield = new List<Hero>(){null, null, null, null, null};
        foreach(var heroView in heroViews){
            if(heroView != null){
                Destroy(heroView.gameObject);
            }
        }
        heroViews.Clear();
        currentHeroSlotIndex = 0;
        battlefieldView.currentHeroSlotView = null;
    }
}
