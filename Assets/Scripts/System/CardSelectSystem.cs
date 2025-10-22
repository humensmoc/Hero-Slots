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
    public void Init(){
        cardSelectPanelView.Init();
        refreshButton.onClick.AddListener(Refresh);
        skipButton.onClick.AddListener(HideCardSelectView);
    }

    public void ShowCardSelectView(){
        cardSelectPanel.SetActive(true);
        cardSelectPanelView.body.SetActive(true);
    }

    public void HideCardSelectView(){
        cardSelectPanel.SetActive(false);
        cardSelectPanelView.body.SetActive(false);
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
