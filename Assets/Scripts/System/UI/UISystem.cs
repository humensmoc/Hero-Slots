using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : Singleton<UISystem>
{
    public GameObject outGameUI;
    
    public RuntimeEffectDataView runtimeEffectDataView;

    public GameOverPanelView gameOverPanelView;

    public SetupPanelView setupPanelView;

    public GameObject inGameShopPanelView;


    public void Init(){
        setupPanelView.Init();
        gameOverPanelView.Init();
    }
}
