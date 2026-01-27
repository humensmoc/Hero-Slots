using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Burst.CompilerServices;

public class HeroSystem : Singleton<HeroSystem>
{
    public BattlefieldView battlefieldView;
    public Transform heroParent;

    public void Init(List<HeroData> heroDatas){

        Model.HeroesInDeck.Add(new Hero(heroDatas[Random.Range(0, heroDatas.Count)]));

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

        Debug.Log("heroView.y: " + heroView.y+" , currentHeroSlotIndex: " + Model.CurrentHeroSlotIndex);
        
        Model.CurrentHeroSlotIndex=battlefieldView.heroSlotViews.IndexOf(heroSlotView);

        // Debug.Log("currentHeroSlotIndex: " + currentHeroSlotIndex);
        // for(int i=0;i<5;i++){
        //     if(battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex]!=null){
        //         battlefieldView.cardViewsInBattlefield[i,currentHeroSlotIndex].transform.DOScale(1.1f,0.15f);
        //     }
        // }
    }
    public void AddHero(Hero hero){
        Model.HeroesInDeck.Add(hero);
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity,0);
        Model.HeroViews.Add(heroView);
    }

    public void RemoveHero(Hero hero){
        Model.HeroesInDeck.Remove(hero);
        HeroView heroView = Model.HeroViews.Find(view => view.hero == hero);
        Model.HeroViews.Remove(heroView);
        Destroy(heroView.gameObject);
    }

    public IEnumerator DrawHero(){
        // 检查战场是否有空位
        List<int> emptyIndexs = new List<int>();
        for(int i = 0; i < Model.HeroesInBattlefield.Count; i++){
            if(Model.HeroesInBattlefield[i] == null)
                emptyIndexs.Add(i);
        }
        if(emptyIndexs.Count==0)
            yield break;

        // 获取空位的索引
        int randomY=emptyIndexs[Random.Range(0, emptyIndexs.Count)];

        Hero hero= Model.HeroesInDeck[Random.Range(0, Model.HeroesInDeck.Count)];
        Model.HeroesInDeck.Remove(hero);
        Model.HeroesInBattlefield[randomY] = hero;

        // 创建英雄视图
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity,randomY);
        Model.HeroViews.Add(heroView);

        Tween tween = heroView.transform.DOLocalMove(heroParent.position + new Vector3(0, randomY * Model.HeroPositionInterval, 0), 0.15f);
        yield return tween.WaitForCompletion();
    }

    public IEnumerator DiscardHero(Hero hero){

        Model.HeroesInDeck.Add(hero);
        // 找到英雄在战场中的索引并设置为null
        int index = Model.HeroesInBattlefield.IndexOf(hero);
        if(index >= 0){
            Model.HeroesInBattlefield[index] = null;
        }
        HeroView heroView = Model.HeroViews.Find(view => view.hero == hero);
        heroView.Remove();
        
        Model.HeroViews.Remove(heroView);
        
        Tween tween = heroView.transform.DOScale(Vector3.zero,0.15f).OnComplete(()=>{
            Destroy(heroView.gameObject);
        });
        yield return tween.WaitForCompletion();
    }

    public IEnumerator DrawAllHero(){
        while(Model.HeroesInDeck.Count > 0){
            yield return DrawHero();
        }
    }

    public IEnumerator DiscardAllHero(){
        for(int i = 0; i < Model.HeroesInBattlefield.Count; i++){
            if(Model.HeroesInBattlefield[i] != null){
                yield return DiscardHero(Model.HeroesInBattlefield[i]);
            }
        }
    }

    public HeroView GetHeroView(Hero hero){
        return Model.HeroViews.Find(view => view.hero == hero);
    }

    public void Reset()
    {
        StopAllCoroutines();
        Model.HeroesInDeck.Clear();
        Model.HeroesInBattlefield = new List<Hero>(){null, null, null, null, null};
        foreach(var heroView in Model.HeroViews){
            if(heroView != null){
                Destroy(heroView.gameObject);
            }
        }
        Model.HeroViews.Clear();
        Model.CurrentHeroSlotIndex = 0;
        battlefieldView.currentHeroSlotView = null;
    }
}
