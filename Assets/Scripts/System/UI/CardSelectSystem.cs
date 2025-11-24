using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CardSelectSystem : Singleton<CardSelectSystem>
{
    public CardSelectPanelView cardSelectPanelView;
    public GameObject cardSelectPanel;
    public GameObject cardSelectItemPrefab;
    public List<CardData> cardDatas=new();
    public int optionCount;

    public Button refreshButton;
    public Button skipButton;

    /// <summary>
    /// 是否显示卡牌选择面板的body
    /// </summary>
    public bool isShow = false;
    
    public void Init(){
        cardSelectPanelView.Init();
        refreshButton.onClick.AddListener(Refresh);
        skipButton.onClick.AddListener(HideCardSelectView);
    }

    public void ShowCardSelectView(){
        cardSelectPanel.SetActive(true);
        cardSelectPanelView.ShowPanel();
    }

    public void HideCardSelectView(){
        cardSelectPanelView.HidePanel();

        if(TurnSystem.Instance.currentTurn==3||TurnSystem.Instance.currentTurn==6){
            HeroSelectSystem.Instance.ShowHeroSelectView();
            HeroSelectSystem.Instance.Refresh();
        }
    }

    public void Refresh(){
        cardSelectPanelView.RemoveAllCardSelectItem();
        cardDatas.Clear();
        CardLibrary.cardDatas.ForEach(cardData => cardDatas.Add(cardData.Clone()));

        for(int i=0;i<optionCount;i++){
            if(cardDatas.Count == 0) break; // 防止从空列表抽取
            
            CardData cardData=cardDatas.Draw();
            cardDatas.Add(cardData);
            cardSelectPanelView.AddCardSelectItem(cardData);
        }

    }

    public void SelectCard(CardData cardData){
        CardSystem.Instance.cardsInDeck.Add(new Card(cardData));
        HideCardSelectView();
    }
}
