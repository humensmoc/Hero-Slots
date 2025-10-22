using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    void Start()
    {
        CardSystem.Instance.Init(CardLibrary.cardDatas);  
        HeroSystem.Instance.Init(HeroLibrary.heroDatas);
        EnemySystem.Instance.Init(EnemyLibrary.enemyDatas[0]);
        CardSelectSystem.Instance.Init();
    }

    
}
