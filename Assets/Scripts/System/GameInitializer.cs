using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    void Start()
    {

        Init();
    }

    public void Init(){
        UISystem.Instance.Init();

        DevToolSystem.Instance.Init();
        CardSystem.Instance.Init(CardLibrary.cardDatas);  
        HeroSystem.Instance.Init(HeroLibrary.heroDatas);
        EnemySystem.Instance.Init(EnemyLibrary.enemyDatas[0]);
        CardSelectSystem.Instance.Init();
        HeroSelectSystem.Instance.Init();
    }

    public void ResetGame(){
        CardSystem.Instance.cardsInDeck.Clear();
        HeroSystem.Instance.heroesInDeck.Clear();
        EnemySystem.Instance.enemies.Clear();
        CardSelectSystem.Instance.cardDatas.Clear();
        HeroSelectSystem.Instance.heroDatas.Clear();
        EventSystem.Instance.ClearActions();

        Init();
    }
}
