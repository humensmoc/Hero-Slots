using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Singleton<GameInitializer>
{
    void Start()
    {
        
        OutGameInit();
    }

    public void OutGameInit(){
        UISystem.Instance.outGameUI.SetActive(true);

        UISystem.Instance.Init();

        EventSystem.Instance.OnGameStart+=()=>{
            UISystem.Instance.Init();
            ResetInGame();
        };
    }

    public void InGameInit(){
        Model.Init();
        DevToolSystem.Instance.Init();
        CardSystem.Instance.Init(CardLibrary.cardDatas);  
        HeroSystem.Instance.Init(HeroLibrary.heroDatas);
        EnemySystem.Instance.Init(EnemyLibrary.testLevelData);
        CardSelectSystem.Instance.Init();
        HeroSelectSystem.Instance.Init();
        DeleteCardPanelView.Instance.Init();
    }

    public void ResetInGame(){
        CardSystem.Instance.Reset();
        HeroSystem.Instance.Reset();
        EnemySystem.Instance.Reset();
        EventSystem.Instance.ClearActions();
        RuntimeEffectData.Reset();
        TurnSystem.Instance.Reset();

        InGameInit();
    }
}
