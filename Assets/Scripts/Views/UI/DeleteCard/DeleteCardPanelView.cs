using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteCardPanelView : Singleton<DeleteCardPanelView>
{
    public Button hideButton;
    public Button DeleteCardButton;
    public Image deleteCardButtonImage;
    public bool isDeleteCardMode = false;
    public GameObject body;
    bool isShow =false;
    public List<DeleteCardItemView> deleteCardItemViews;
    public Transform deleteCardItemParent;
    public GameObject deleteCardItemPrefab;

    public void Init(){
        hideButton.onClick.AddListener(SwitchVisibility);
        DeleteCardButton.onClick.AddListener(()=>{
            isDeleteCardMode = !isDeleteCardMode;

            RefreshUI();
        });
        // body.SetActive(false);
        body.SetActive(false);

    }

    public void Show(){
        gameObject.SetActive(true);
        body.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
        isDeleteCardMode = false;
    }

    public void RefreshUI(){
        if(isDeleteCardMode){
            deleteCardButtonImage.color = Color.red;
        }else{
            deleteCardButtonImage.color = Color.white;
        }

        RemoveAllDeleteCardItem();

        if(CardSystem.Instance.cardsInDeck.Count == 0){
            return;
        }

        foreach(Card card in CardSystem.Instance.cardsInDeck){
            AddDeleteCardItem(card);
        }
    }

    public void SwitchVisibility(){
        if(isShow){
            body.SetActive(false);
            isShow = false;
            
        }else{
            body.SetActive(true);
            isShow = true;

            RefreshUI();
        }
    }

    public void AddDeleteCardItem(Card card){
        DeleteCardItemView deleteCardItemView = Instantiate(deleteCardItemPrefab,deleteCardItemParent).GetComponent<DeleteCardItemView>();
        deleteCardItemView.Init(card);
        deleteCardItemViews.Add(deleteCardItemView);
    }

    public void RemoveAllDeleteCardItem(){
        foreach(DeleteCardItemView deleteCardItemView in deleteCardItemViews){
            Destroy(deleteCardItemView.gameObject);
        }
        deleteCardItemViews.Clear();
    }
    
}