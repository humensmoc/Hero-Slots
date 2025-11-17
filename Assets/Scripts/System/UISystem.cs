using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : Singleton<UISystem>
{
    public RuntimeEffectDataView runtimeEffectDataView;

    public GameOverPanelView gameOverPanelView;

    public SetupPanelView setupPanelView;


    public void Init(){
        setupPanelView.Init();
        gameOverPanelView.Init();
    }
}
