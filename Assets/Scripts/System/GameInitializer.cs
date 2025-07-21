using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    [SerializeField] List<CardData> testCardDatas;
    [SerializeField] HeroData testHeroData;
    public BulletData testBulletData;
    [SerializeField] EnemyData testEnemyData;
    void Start()
    {
        CardSystem.Instance.Init(testCardDatas);  
        Hero hero = new Hero(testHeroData);
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity);
        HeroSlotSystem.Instance.MoveToSlot(heroView,HeroSlotSystem.Instance.battlefieldView.heroSlotViews[0]);
        EnemySystem.Instance.Init(testEnemyData);
    }

    
}
