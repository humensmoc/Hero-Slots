using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectPanelView : MonoBehaviour
{
    public List<HeroSelectItemView> heroSelectItemViews;
    public Transform heroSelectItemParent;
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

    public void AddHeroSelectItem(HeroData heroData){
        HeroSelectItemView heroSelectItemView = Instantiate(HeroSelectSystem.Instance.heroSelectItemPrefab,heroSelectItemParent).GetComponent<HeroSelectItemView>();
        heroSelectItemView.Init(heroData);
        heroSelectItemViews.Add(heroSelectItemView);
    }
    public void RemoveAllHeroSelectItem(){
        for(int i=0;i<heroSelectItemViews.Count;i++){
            Destroy(heroSelectItemViews[i].gameObject);
        }
        heroSelectItemViews.Clear();
    }
}