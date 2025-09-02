using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    [SerializeField] public  List<CardData> testCardDatas;
    [SerializeField] public List<HeroData> testHeroDatas;
    [SerializeField] public List<BulletData> testBulletDatas;
    public EnemyData testEnemyData;
    void Start()
    {
        CardSystem.Instance.Init(testCardDatas);  
        HeroSystem.Instance.Init(testHeroDatas);
        EnemySystem.Instance.Init(testEnemyData);
        CardSelectSystem.Instance.Init();
    }

    
}
