using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    [SerializeField] List<CardData> testCardDatas;
    [SerializeField] HeroData testHeroData;
    public BulletData testBulletData;
    public EnemyData testEnemyData;
    void Start()
    {
        CardSystem.Instance.Init(testCardDatas);  
        HeroSystem.Instance.Init(testHeroData);
        EnemySystem.Instance.Init(testEnemyData);
    }

    
}
