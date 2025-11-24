using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class HeroSelectSystem : Singleton<HeroSelectSystem>
{
    public HeroSelectPanelView heroSelectPanelView;
    public GameObject heroSelectPanel;
    public GameObject heroSelectItemPrefab;
    public List<HeroData> heroDatas=new();
    public int optionCount;

    public Button refreshButton;
    public Button skipButton;
    public void Init(){
        heroSelectPanelView.Init();
        refreshButton.onClick.AddListener(Refresh);
        skipButton.onClick.AddListener(HideHeroSelectView);
    }

    public void ShowHeroSelectView(){
        heroSelectPanel.SetActive(true);
        heroSelectPanelView.body.SetActive(true);
    }

    public void HideHeroSelectView(){
        heroSelectPanel.SetActive(false);
        heroSelectPanelView.body.SetActive(false);
    }

    public void Refresh(){
        heroSelectPanelView.RemoveAllHeroSelectItem();
        heroDatas.Clear();
        HeroLibrary.heroDatas.ForEach(heroData => heroDatas.Add(heroData.Clone()));

        for(int i=0;i<optionCount;i++){
            if(heroDatas.Count == 0) break; // 防止从空列表抽取
            
            HeroData heroData=heroDatas.Draw();
            heroDatas.Add(heroData);
            heroSelectPanelView.AddHeroSelectItem(heroData);
        }

    }

    public void SelectHero(HeroData heroData){
        HeroSystem.Instance.heroesInDeck.Add(new Hero(heroData));
        HideHeroSelectView();
    }
}
