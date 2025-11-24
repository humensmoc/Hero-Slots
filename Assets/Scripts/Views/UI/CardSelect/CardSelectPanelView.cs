using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectPanelView : MonoBehaviour
{
    public List<CardSelectItemView> cardSelectItemViews;
    public Transform cardSelectItemParent;
    public Button hideButton;
    public GameObject panel;
    public GameObject body;
    bool isShow =true;

    public void Init(){
        hideButton.onClick.AddListener(SwitchVisibility);
        // body.SetActive(false);
        HidePanel();
    }

    public void ShowPanel(){
        panel.SetActive(true);
        body.SetActive(true);
        CardSelectSystem.Instance.isShow = true;
    }

    public void HidePanel(){
        panel.SetActive(false);
        CardSelectSystem.Instance.isShow = false;
    }

    public void SwitchVisibility(){
        if(isShow){
            body.SetActive(false);
            CardSelectSystem.Instance.isShow = false;
            isShow = false;
        }else{
            body.SetActive(true);
            CardSelectSystem.Instance.isShow = true;
            isShow = true;
        }
    }

    public void AddCardSelectItem(CardData cardData){
        CardSelectItemView cardSelectItemView = Instantiate(CardSelectSystem.Instance.cardSelectItemPrefab,cardSelectItemParent).GetComponent<CardSelectItemView>();
        cardSelectItemView.Init(cardData);
        cardSelectItemViews.Add(cardSelectItemView);
    }
    public void RemoveAllCardSelectItem(){
        for(int i=0;i<cardSelectItemViews.Count;i++){
            Destroy(cardSelectItemViews[i].gameObject);
        }
        cardSelectItemViews.Clear();
    }
}