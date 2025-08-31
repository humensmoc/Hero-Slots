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
    public GameObject body;
    bool isShow =true;

    public void Init(){
        hideButton.onClick.AddListener(Hide);
        body.SetActive(false);
    }

    public void Hide(){
        if(isShow){
            body.SetActive(false);
            isShow = false;
        }else{
            body.SetActive(true);
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